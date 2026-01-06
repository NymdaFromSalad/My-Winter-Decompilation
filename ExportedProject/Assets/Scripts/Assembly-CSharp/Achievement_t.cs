public class Achievement_t
{
	public string m_strAchievementID;

	public string m_strName;

	public string m_strDescription;

	public bool m_bAchieved;

	public Achievement_t(string achievementID)
	{
		m_strAchievementID = achievementID;
		m_strName = string.Empty;
		m_strDescription = string.Empty;
		m_bAchieved = false;
	}
}
