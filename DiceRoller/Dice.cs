using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text.RegularExpressions;

namespace DiceRoller
{
    public class Dice
    {

        Random _rand = new Random();

        public List<Int32> Roll(int sides, int numRolls)
        {
            List<int> Rolls = new List<int>();
            for (int i = 0; i < numRolls; i++)
            {
                Rolls.Add(_rand.Next(1, sides + 1));
            }
            return Rolls;
        }

        public List<Int32> DropLowest(List<Int32> rolls)
        {
            Int32 lowestIndex = rolls.IndexOf(rolls.Min());
            rolls.RemoveAt(lowestIndex);
            return rolls;
        }

        public int Evaluate(string expression)
        {
            DataTable table = new DataTable();
            table.Columns.Add("expression", typeof(string), expression);
            DataRow row = table.NewRow();
            table.Rows.Add(row);
            return int.Parse((string)row["expression"]);
        }

        public List<List<Int32>> Parse(string diestring)
        {
            List<List<Int32>> listList = new List<List<Int32>>();

            diestring = diestring.Replace("-","+ -");
            diestring = diestring.Replace("%", "100");
            diestring = diestring.ToLower();
            var items = diestring.Split('+');
            var res = new List<int>();
            var type = new List<int>();
            int num = new int();
            int max = new int();
            for (var i = 0; i < items.Length; i++)
            {
                var match = Regex.Match(items[i], @"^[ \t]*(-)?(\d+)?(?:(d)(\d+))?(l)?[ \t]*$");
                    if (match.Success) {
                        var sign = match.Groups[1].Success == true?-1:1;
                        int.TryParse(match.Groups[2].Value, out num);
                        int.TryParse(match.Groups[4].Value, out max);
                        if (match.Groups[3].Success == true) {
                            num = num > 0 ? num : 1;
                            res.AddRange(Roll(max,num));
                            for ( int j=1; j<=num; j++) {
                                type.Add(max);
                            }
                        }
                        else if (num > 0) {

                            res.Add(sign * num);
                            type.Add(0);
                        }
                        if (match.Groups[5].Success == true)
                        {
                            res = DropLowest(res);
                            type.Remove(type.LastOrDefault());
                        }
                    } 
                    
            }
            if (res.Count() == 0) return null;
            listList.Add(res);
            listList.Add(type);
            return listList;
        }

    }
}