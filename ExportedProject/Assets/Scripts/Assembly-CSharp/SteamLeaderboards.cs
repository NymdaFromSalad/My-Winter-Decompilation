using Steamworks;
using UnityEngine;

public class SteamLeaderboards : MonoBehaviour
{
	private Leaderboard_t[] m_Leaderboards;

	private CallResult<LeaderboardFindResult_t> m_callResultFindLeaderboard;

	private CallResult<LeaderboardScoreUploaded_t> m_callResultUploadScore;

	private bool m_bLoading;

	public void Init()
	{
		m_callResultFindLeaderboard = new CallResult<LeaderboardFindResult_t>(OnFindLeaderboard);
		m_callResultUploadScore = new CallResult<LeaderboardScoreUploaded_t>(OnScoreUploaded);
	}

	public void InitLeaderboards(string[] names)
	{
		if (m_Leaderboards != null)
		{
			Debug.LogWarning("Attempted to InitLeaderboards twice.");
			return;
		}
		if (names == null || names.Length == 0)
		{
			Debug.LogWarning("Empty list of Leaderboard names passed into InitLeaderboards.");
			return;
		}
		m_Leaderboards = new Leaderboard_t[names.Length];
		for (int i = 0; i < names.Length; i++)
		{
			m_Leaderboards[i] = new Leaderboard_t(names[i]);
		}
		FindLeaderboards(0);
	}

	public void UploadScore(string name, int score)
	{
		if (!SteamManager.Initialized || m_Leaderboards == null)
		{
			return;
		}
		if (m_bLoading)
		{
			Debug.LogError("Tried to upload score but Leaderboards haven't finished loading yet.");
			return;
		}
		Leaderboard_t[] leaderboards = m_Leaderboards;
		foreach (Leaderboard_t leaderboard_t in leaderboards)
		{
			if (leaderboard_t.m_strName == name)
			{
				SteamAPICall_t hAPICall = SteamUserStats.UploadLeaderboardScore(leaderboard_t.m_hHandle, ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest, score, null, 0);
				Debug.Log("New Score of " + score + " uploaded to leaderboard: " + name);
				m_callResultUploadScore.Set(hAPICall);
				return;
			}
		}
		Debug.LogError("UploadScore could not find leaderboard named: " + name);
	}

	private void FindLeaderboards(int index)
	{
		if (index > m_Leaderboards.Length)
		{
			Debug.LogWarning("SteamLeaderboards Script Internal Error - index > m_Leaderboards.Length in FindLeaderboards.");
			return;
		}
		SteamAPICall_t hAPICall = SteamUserStats.FindLeaderboard(m_Leaderboards[index].m_strName);
		m_callResultFindLeaderboard.Set(hAPICall);
		m_bLoading = true;
	}

	private void OnFindLeaderboard(LeaderboardFindResult_t pFindLeaderboardResult, bool bIOFailure)
	{
		if (bIOFailure)
		{
			return;
		}
		if (pFindLeaderboardResult.m_bLeaderboardFound == 0)
		{
			Debug.LogWarning("Could not find one of the Leaderboards, was it named correctly?");
			return;
		}
		string leaderboardName = SteamUserStats.GetLeaderboardName(pFindLeaderboardResult.m_hSteamLeaderboard);
		int i;
		for (i = 0; i < m_Leaderboards.Length; i++)
		{
			if (leaderboardName == m_Leaderboards[i].m_strName)
			{
				m_Leaderboards[i].m_hHandle = pFindLeaderboardResult.m_hSteamLeaderboard;
				break;
			}
		}
		if (i + 1 < m_Leaderboards.Length)
		{
			FindLeaderboards(i + 1);
		}
		else
		{
			m_bLoading = false;
		}
	}

	private void OnScoreUploaded(LeaderboardScoreUploaded_t pScoreUploadedResult, bool bIOFailure)
	{
		Debug.Log("OnScoreUploaded " + pScoreUploadedResult.m_bSuccess + " - " + pScoreUploadedResult.m_bScoreChanged);
	}
}
