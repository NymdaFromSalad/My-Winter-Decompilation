using System;

public class Stat_t
{
	public string m_strStatName;

	public Type m_StatType;

	public float m_fValue;

	public int m_nValue;

	public Stat_t(string name, Type type)
	{
		m_strStatName = name;
		m_StatType = type;
	}
}
