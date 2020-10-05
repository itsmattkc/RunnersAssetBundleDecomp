using System;

public class ResultObjParam
{
	public string scoreLabel;

	public string chaoBonusLabel;

	public string chaoBonusTexture;

	public string charaBonusLabel;

	public string charaBonusTexture;

	public string campaignBonusLabel;

	public string chaoCountBonusLabel;

	public string charaBonusRankScore;

	public ResultObjParam(string s1, string s2, string s3, string s4, string s5, string s6, string s7, string s8)
	{
		this.scoreLabel = s1;
		this.chaoBonusLabel = s2;
		this.chaoBonusTexture = s3;
		this.charaBonusLabel = s4;
		this.charaBonusTexture = s5;
		this.campaignBonusLabel = s6;
		this.chaoCountBonusLabel = s7;
		this.charaBonusRankScore = s8;
	}
}
