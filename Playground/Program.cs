using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = File.ReadAllText(@"data\test.txt");
            Console.WriteLine(PartOne(input));
        }

        protected static int PartOne(string input)
        {
            var ruleStrings = input.Split('\n');
            List<string> checkedBags = new List<string>();
            List<Rule> rules = new List<Rule>();
            foreach (var ruleString in ruleStrings)
            {
                var parts = ruleString.Split(" bags contain ");
                var containedRules = new Dictionary<string, int>();
                if (!parts[1].Contains("no other bags."))
                {
                    var containedRulesStrings = parts[1];
                    containedRulesStrings = containedRulesStrings.Remove(containedRulesStrings.Length - 2);
                    var containedRulesArray = containedRulesStrings.Split(',').Select(x => x.Trim());
                    foreach (var rule in containedRulesArray)
                    {
                        int spaceIndex = rule.IndexOf(' ');
                        containedRules.Add(rule.Substring(spaceIndex + 1).Replace(" bags", "").Replace(" bag",""), int.Parse(rule.Substring(0, spaceIndex)));
                    }
                }
                rules.Add(new Rule() { Name = parts[0], ContainedRules = containedRules });
            }
            Console.WriteLine("===== Start of recursive method =====");
            return GetResult(new List<string>() { "shiny gold" }, rules, new List<string>());
        }

        static int GetResult(List<string> bagsToCheck, List<Rule> rules, List<string> checkedBags)
        {
            List<string> nextBags = new List<string>();
            Console.WriteLine("Bags to check: " + String.Join(',', bagsToCheck));
            Console.WriteLine("Checked bags: " + String.Join(',', checkedBags));
            foreach (var rule in rules)
            {
                Console.WriteLine($"Rule {rule.Name}:{String.Join(',', rule.ContainedRules.Keys)}");
                if (!checkedBags.Contains(rule.Name) && rule.ContainedRules.Keys.Any(key => bagsToCheck.Contains(key)))
                {
                    nextBags.Add(rule.Name);
                    checkedBags.Add(rule.Name);
                }
            }
            if (nextBags.Count == 0)
            {
                return checkedBags.Count;
            }
            else
            {
                return GetResult(nextBags, rules, checkedBags);
            }
        }

        private class Rule
        {
            public string Name { get; set; }
            public Dictionary<string, int> ContainedRules { get; set; }
        }
    }
}
