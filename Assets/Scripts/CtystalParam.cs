using System;

public class CtystalParam
{
	public bool m_big;

	public string m_objName;

	public string m_model;

	public string m_effect;

	public string m_se;

	public int m_score;

	public bool m_scoreIcon;

	public CtystalParam(bool big, string objName, string model, string effect, string se, int score, bool scoreIcon)
	{
		this.m_big = big;
		this.m_objName = objName;
		this.m_model = model;
		this.m_effect = effect;
		this.m_se = se;
		this.m_score = score;
		this.m_scoreIcon = scoreIcon;
	}
}
