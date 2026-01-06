using System;
using Steamworks;
using UnityEngine;

[AddComponentMenu("Scripts/Steam Simple Scripts/Stats and Achievements")]
public class SteamStatsAndAchievements : MonoBehaviour
{
	private Achievement_t[] m_Achievements;

	private Stat_t[] m_Stats;

	private CGameID m_GameID;

	private bool m_bRequestedStats;

	private bool m_bStatsValid;

	private bool m_bStoreStats;

	protected Callback<UserStatsReceived_t> m_UserStatsReceived;

	protected Callback<UserStatsStored_t> m_UserStatsStored;

	public void Init()
	{
		if (SteamManager.Initialized)
		{
			m_GameID = new CGameID(SteamUtils.GetAppID());
			m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
			m_UserStatsStored = Callback<UserStatsStored_t>.Create(OnUserStatsStored);
			m_bRequestedStats = false;
			m_bStatsValid = false;
		}
	}

	public void InitAchievements(string[] ach)
	{
		if (m_Achievements != null)
		{
			Debug.LogWarning("Attempted to InitAchievements twice.");
			return;
		}
		m_Achievements = new Achievement_t[ach.Length];
		for (int i = 0; i < ach.Length; i++)
		{
			m_Achievements[i] = new Achievement_t(ach[i]);
		}
	}

	public void InitStats(Stat_t[] stats)
	{
		if (m_Stats != null)
		{
			Debug.LogWarning("Attempted to InitStats twice.");
		}
		else
		{
			m_Stats = stats;
		}
	}

	public bool UnlockAchievement(string achievementId)
	{
		if (m_Achievements == null)
		{
			return false;
		}
		bool result = false;
		Achievement_t[] achievements = m_Achievements;
		foreach (Achievement_t achievement_t in achievements)
		{
			if (achievement_t.m_strAchievementID == achievementId)
			{
				if (!achievement_t.m_bAchieved)
				{
					achievement_t.m_bAchieved = true;
					result = SteamUserStats.SetAchievement(achievement_t.m_strAchievementID);
					m_bStoreStats = true;
				}
				else
				{
					result = true;
				}
				break;
			}
		}
		return result;
	}

	public Achievement_t GetAchievement(string achievementId)
	{
		if (m_Achievements == null)
		{
			return null;
		}
		Achievement_t[] achievements = m_Achievements;
		foreach (Achievement_t achievement_t in achievements)
		{
			if (achievement_t.m_strAchievementID == achievementId)
			{
				return achievement_t;
			}
		}
		Debug.LogError("GetAchievement could not find achievement: " + achievementId);
		return null;
	}

	public Stat_t GetStat(string statname)
	{
		if (m_Achievements == null)
		{
			return null;
		}
		Stat_t[] stats = m_Stats;
		foreach (Stat_t stat_t in stats)
		{
			if (stat_t.m_strStatName == statname)
			{
				return stat_t;
			}
		}
		Debug.LogError("GetStat could not find stat: " + statname);
		return null;
	}

	public void SetStat(string statname, float value)
	{
		if (m_Achievements == null)
		{
			return;
		}
		Stat_t[] stats = m_Stats;
		foreach (Stat_t stat_t in stats)
		{
			if (stat_t.m_strStatName == statname)
			{
				if (stat_t.m_StatType != typeof(float))
				{
					throw new Exception("Trying to assign an int to a float stat: " + statname);
				}
				stat_t.m_fValue = value;
				m_bStoreStats = true;
				return;
			}
		}
		Debug.LogError("SetStat could not find stat: " + statname);
	}

	public void SetStat(string statname, int value)
	{
		if (m_Achievements == null)
		{
			return;
		}
		Stat_t[] stats = m_Stats;
		foreach (Stat_t stat_t in stats)
		{
			if (stat_t.m_strStatName == statname)
			{
				if (stat_t.m_StatType != typeof(int))
				{
					throw new Exception("Trying to assign an float to a int stat: " + statname);
				}
				stat_t.m_nValue = value;
				m_bStoreStats = true;
				return;
			}
		}
		Debug.LogError("GetStat could not find stat: " + statname);
	}

	public bool ResetStatsAndAchievements()
	{
		m_bRequestedStats = false;
		m_bStatsValid = false;
		return SteamUserStats.ResetAllStats(true);
	}

	private void Update()
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		if (!m_bRequestedStats && m_Achievements != null && m_Stats != null)
		{
			m_bRequestedStats = SteamUserStats.RequestCurrentStats();
		}
		if (!m_bStatsValid || !m_bStoreStats)
		{
			return;
		}
		if (m_Stats != null)
		{
			for (int i = 0; i < m_Stats.Length; i++)
			{
				if (!((m_Stats[i].m_StatType != typeof(float)) ? SteamUserStats.SetStat(m_Stats[i].m_strStatName, m_Stats[i].m_nValue) : SteamUserStats.SetStat(m_Stats[i].m_strStatName, m_Stats[i].m_fValue)))
				{
					Debug.LogWarning(" SteamUserStats.SetStat failed for Stat " + m_Stats[i].m_strStatName + "\nIs it registered in the Steam Partner site?");
				}
			}
		}
		m_bStoreStats = !SteamUserStats.StoreStats();
	}

	private void OnUserStatsReceived(UserStatsReceived_t pCallback)
	{
		if ((ulong)m_GameID != pCallback.m_nGameID)
		{
			return;
		}
		if (pCallback.m_eResult != EResult.k_EResultOK)
		{
			Debug.Log("RequestStats - failed, " + pCallback.m_eResult);
			return;
		}
		Achievement_t[] achievements = m_Achievements;
		foreach (Achievement_t achievement_t in achievements)
		{
			if (SteamUserStats.GetAchievement(achievement_t.m_strAchievementID, out achievement_t.m_bAchieved))
			{
				achievement_t.m_strName = SteamUserStats.GetAchievementDisplayAttribute(achievement_t.m_strAchievementID, "name");
				achievement_t.m_strDescription = SteamUserStats.GetAchievementDisplayAttribute(achievement_t.m_strAchievementID, "desc");
			}
			else
			{
				Debug.LogWarning("SteamUserStats.GetAchievement failed for Achievement " + achievement_t.m_strAchievementID + "\nIs it registered in the Steam Partner site?");
			}
		}
		if (m_Stats != null)
		{
			for (int j = 0; j < m_Stats.Length; j++)
			{
				if (!((m_Stats[j].m_StatType != typeof(float)) ? SteamUserStats.GetStat(m_Stats[j].m_strStatName, out m_Stats[j].m_nValue) : SteamUserStats.GetStat(m_Stats[j].m_strStatName, out m_Stats[j].m_fValue)))
				{
					Debug.LogWarning(" SteamUserStats.GetStat failed for Stat " + m_Stats[j].m_strStatName + "\nIs it registered in the Steam Partner site?");
				}
			}
		}
		m_bStatsValid = true;
	}

	private void OnUserStatsStored(UserStatsStored_t pCallback)
	{
		if ((ulong)m_GameID == pCallback.m_nGameID && pCallback.m_eResult != EResult.k_EResultOK)
		{
			if (pCallback.m_eResult == EResult.k_EResultInvalidParam)
			{
				Debug.LogWarning("StoreStats - some failed to validate");
				OnUserStatsReceived(new UserStatsReceived_t
				{
					m_eResult = EResult.k_EResultOK,
					m_nGameID = (ulong)m_GameID
				});
			}
			else
			{
				Debug.LogWarning("StoreStats - failed, " + pCallback.m_eResult);
			}
		}
	}
}
