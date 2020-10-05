using System;
using System.Collections.Generic;

public class RegionInfoTable
{
	private List<RegionInfo> m_regionInfoList;

	public RegionInfoTable()
	{
		this.m_regionInfoList = new List<RegionInfo>();
		this.m_regionInfoList.Add(new RegionInfo(1, "JP", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(2, "US", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(3, "GB", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(4, "FR", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(5, "IT", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(6, "DE", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(7, "ES", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(8, "RU", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(9, "PT", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(10, "BR", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(11, "CA", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(12, "AU", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(13, "KR", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(14, "CN", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(15, "TW", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(16, "HK", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(17, "AT", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(18, "BE", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(19, "BG", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(20, "DK", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(21, "FI", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(22, "GR", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(23, "HU", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(24, "IS", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(25, "IE", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(26, "NL", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(27, "NO", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(28, "PL", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(29, "SE", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(30, "CH", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(31, "IN", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(32, "ID", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(33, "IL", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(34, "MY", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(35, "PH", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(36, "SG", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(37, "TH", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(38, "TR", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(39, "VN", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(40, "AR", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(41, "CL", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(42, "CO", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(43, "MX", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(44, "NZ", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(45, "EG", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(46, "AL", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(47, "DZ", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(48, "AO", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(49, "AI", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(50, "AG", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(51, "AM", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(52, "AW", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(53, "AZ", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(54, "BS", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(55, "BH", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(56, "BD", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(57, "BB", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(58, "BY", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(59, "BZ", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(60, "BJ", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(61, "BM", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(62, "BT", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(63, "BO", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(64, "BA", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(65, "BW", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(66, "BN", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(67, "BF", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(68, "KH", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(69, "CM", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(70, "CV", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(71, "KY", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(72, "TD", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(73, "CD", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(74, "CR", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(75, "CI", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(76, "HR", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(77, "CY", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(78, "CZ", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(79, "DM", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(80, "DO", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(81, "EC", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(82, "SV", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(83, "EE", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(84, "FJ", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(85, "GA", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(86, "GM", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(87, "GH", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(88, "GD", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(89, "GT", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(90, "GW", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(91, "GY", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(92, "HT", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(93, "HN", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(94, "IR", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(95, "JM", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(96, "JO", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(97, "KZ", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(98, "KE", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(99, "KW", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(100, "KG", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(101, "LA", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(102, "LV", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(103, "LB", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(104, "LR", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(105, "LT", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(106, "LU", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(107, "MO", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(108, "MK", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(109, "MG", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(110, "MW", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(111, "ML", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(112, "MT", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(113, "MR", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(114, "MU", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(115, "FM", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(116, "MD", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(117, "MN", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(118, "MS", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(119, "MA", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(120, "MZ", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(121, "MM", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(122, "NA", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(123, "NP", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(124, "AN", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(125, "NI", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(126, "NE", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(127, "NG", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(128, "OM", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(129, "PK", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(130, "PW", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(131, "PA", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(132, "PG", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(133, "PY", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(134, "PE", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(135, "QA", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(136, "RO", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(137, "RW", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(138, "KN", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(139, "LC", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(140, "VC", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(141, "SA", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(142, "SN", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(143, "RS", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(144, "SC", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(145, "SL", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(146, "SK", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(147, "SI", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(148, "SB", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(149, "ZA", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(150, "LK", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(151, "SR", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(152, "SZ", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(153, "ST", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(154, "TJ", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(155, "TZ", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(156, "TG", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(157, "TT", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(158, "TN", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(159, "TM", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(160, "TC", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(161, "UG", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(162, "UA", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(163, "AE", "NON", "NON"));
		this.m_regionInfoList.Add(new RegionInfo(164, "UY", "NON", "ESRB"));
		this.m_regionInfoList.Add(new RegionInfo(165, "UZ", "NON", "NON"));
	}

	public RegionInfo GetInfo(int index)
	{
		if (index >= this.m_regionInfoList.Count)
		{
			return null;
		}
		return this.m_regionInfoList[index];
	}

	public RegionInfo GetInfo(string countryCode)
	{
		for (int i = 0; i < this.m_regionInfoList.Count; i++)
		{
			if (countryCode == this.m_regionInfoList[i].CountryCode)
			{
				return this.m_regionInfoList[i];
			}
		}
		return null;
	}
}
