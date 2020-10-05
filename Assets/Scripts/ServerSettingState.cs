using System;

public class ServerSettingState
{
	public long m_energyRefreshTime;

	public int m_energyRecoveryMax;

	public int m_onePlayCmCount;

	public int m_onePlayContinueCount;

	public int m_cmSkipCount;

	public bool m_isPurchased;

	public ServerItemState m_invitBaseIncentive;

	public ServerItemState m_rentalBaseIncentive;

	public int m_subCharaRingPayment;

	public string m_userName;

	public string m_userId;

	public int m_monthPurchase;

	public string m_birthday;

	public int m_countryId;

	public string m_countryCode;

	public ServerSettingState()
	{
		this.m_energyRefreshTime = 0L;
		this.m_energyRecoveryMax = 1;
		this.m_invitBaseIncentive = new ServerItemState();
		this.m_rentalBaseIncentive = new ServerItemState();
		this.m_subCharaRingPayment = 300;
		this.m_userName = string.Empty;
		this.m_userId = string.Empty;
		this.m_monthPurchase = 0;
		this.m_birthday = string.Empty;
		this.m_countryId = 0;
		this.m_countryCode = string.Empty;
		this.m_onePlayCmCount = 0;
		this.m_onePlayContinueCount = 0;
		this.m_isPurchased = false;
	}

	public void CopyTo(ServerSettingState to)
	{
		to.m_energyRefreshTime = this.m_energyRefreshTime;
		to.m_energyRecoveryMax = this.m_energyRecoveryMax;
		this.m_invitBaseIncentive.CopyTo(to.m_invitBaseIncentive);
		this.m_rentalBaseIncentive.CopyTo(to.m_rentalBaseIncentive);
		to.m_subCharaRingPayment = this.m_subCharaRingPayment;
		to.m_userName = string.Copy(this.m_userName);
		to.m_userId = string.Copy(this.m_userId);
		to.m_monthPurchase = this.m_monthPurchase;
		to.m_birthday = this.m_birthday;
		to.m_countryId = this.m_countryId;
		to.m_countryCode = this.m_countryCode;
		to.m_onePlayCmCount = this.m_onePlayCmCount;
		to.m_onePlayContinueCount = this.m_onePlayContinueCount;
		to.m_isPurchased = this.m_isPurchased;
	}
}
