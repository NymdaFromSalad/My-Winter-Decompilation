using System;
using System.Text;
using Steamworks;
using UnityEngine;

internal class SteamManager : MonoBehaviour
{
	private static SteamManager s_instance;

	private bool m_bInitialized;

	private SteamStatsAndAchievements m_StatsAndAchievements;

	private SteamLeaderboards m_Leaderboards;

	private SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;

	private static SteamManager Instance
	{
		get
		{
			return s_instance ?? new GameObject("SteamManager").AddComponent<SteamManager>();
		}
	}

	public static bool Initialized
	{
		get
		{
			return Instance.m_bInitialized;
		}
	}

	public static SteamStatsAndAchievements StatsAndAchievements
	{
		get
		{
			return Instance.m_StatsAndAchievements;
		}
	}

	public static SteamLeaderboards Leaderboards
	{
		get
		{
			return Instance.m_Leaderboards;
		}
	}

	private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning(pchDebugText);
	}

	private void Awake()
	{
		if (s_instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		s_instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (!Packsize.Test())
		{
			Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
		}
		try
		{
			if (SteamAPI.RestartAppIfNecessary(AppId_t.Invalid))
			{
				Application.Quit();
				return;
			}
		}
		catch (DllNotFoundException ex)
		{
			Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + ex, this);
			Application.Quit();
			return;
		}
		m_StatsAndAchievements = base.gameObject.AddComponent<SteamStatsAndAchievements>();
		m_Leaderboards = base.gameObject.AddComponent<SteamLeaderboards>();
		m_bInitialized = SteamAPI.Init();
		if (!m_bInitialized)
		{
			Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);
		}
	}

	private void OnEnable()
	{
		if (s_instance == null)
		{
			s_instance = this;
		}
		if (m_bInitialized)
		{
			if (m_SteamAPIWarningMessageHook == null)
			{
				m_SteamAPIWarningMessageHook = SteamAPIDebugTextHook;
				SteamClient.SetWarningMessageHook(m_SteamAPIWarningMessageHook);
			}
			m_StatsAndAchievements.Init();
			m_Leaderboards.Init();
		}
	}

	private void OnDestroy()
	{
		if (!(s_instance != this))
		{
			s_instance = null;
			if (m_bInitialized)
			{
				SteamAPI.Shutdown();
			}
		}
	}

	private void Update()
	{
		if (m_bInitialized)
		{
			SteamAPI.RunCallbacks();
		}
	}
}
