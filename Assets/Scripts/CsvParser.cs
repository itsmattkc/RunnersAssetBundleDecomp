using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CsvParser
{
	public class CsvFields
	{
		public List<string> field = new List<string>();

		public List<string> FieldList
		{
			get
			{
				return this.field;
			}
		}
	}

	public static List<CsvParser.CsvFields> ParseCsvFromText(string i_text)
	{
		return CsvParser.CsvToArrayList(i_text);
	}

	private static List<CsvParser.CsvFields> CsvToArrayList(string csvText)
	{
		List<CsvParser.CsvFields> list = new List<CsvParser.CsvFields>();
		csvText = csvText.Trim(new char[]
		{
			'\r',
			'\n'
		});
		Regex regex = new Regex("^.*(?:\\n|$)", RegexOptions.Multiline);
		Regex regex2 = new Regex("\\s*(\"(?:[^\"]|\"\")*\"|[^,]*)\\s*,", RegexOptions.None);
		Match match = regex.Match(csvText);
		string text = string.Empty;
		while (match.Success)
		{
			text = match.Value;
			while (CsvParser.CountString(text, "\"") % 2 == 1)
			{
				match = match.NextMatch();
				if (!match.Success)
				{
				}
				text += match.Value;
			}
			text = text.TrimEnd(new char[]
			{
				'\r',
				'\n'
			});
			text += ",";
			CsvParser.CsvFields csvFields = new CsvParser.CsvFields();
			Match match2 = regex2.Match(text);
			while (match2.Success)
			{
				string text2 = match2.Groups[1].Value;
				text2 = text2.Trim();
				if (text2.StartsWith("\"") && text2.EndsWith("\""))
				{
					text2 = text2.Substring(1, text2.Length - 2);
					text2 = text2.Replace("\"\"", "\"");
				}
				match2 = match2.NextMatch();
				if (text2.IndexOf('#') == 0)
				{
					break;
				}
				csvFields.field.Add(text2);
			}
			if (csvFields.field.Count != 0)
			{
				list.Add(csvFields);
			}
			match = match.NextMatch();
		}
		return list;
	}

	private static int CountString(string strInput, string strFind)
	{
		int num = 0;
		for (int i = strInput.IndexOf(strFind); i > -1; i = strInput.IndexOf(strFind, i + 1))
		{
			num++;
		}
		return num;
	}
}
