using Steamworks;

public class Leaderboard_t
{
	public string m_strName;

	public SteamLeaderboard_t m_hHandle;

	public Leaderboard_t(string name)
	{
		m_strName = name;
	}
}
