﻿namespace PersonalFinanceMVC.Views.Compound
{
    public class CalculateVM
    {
        public decimal Principal { get; set; }
        public decimal Rate { get; set; }
        public int Years { get; set; }
        public int Compounds { get; set; }
        public List<CompoundInterestResult> Results { get; set; } = new List<CompoundInterestResult>();

        public class CompoundInterestResult
        {
            public int Year { get; set; }
            public decimal Principal { get; set; }
            public decimal Interest { get; set; }
            public decimal Amount { get; set; }
        }
    }
}