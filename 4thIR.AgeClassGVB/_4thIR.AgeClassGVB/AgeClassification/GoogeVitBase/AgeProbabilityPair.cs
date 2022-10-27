using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4thIR.AgeClassGVB.AgeClassification.GoogeVitBase
{
    public class AgeProbabilityPair
    {
        public AgeProbabilityPair(string ageRange, double probability)
        {
            AgeRange = ageRange;
            Probability = probability;

        }

        public string AgeRange { get; set; }
        public double Probability { get; set; }

    }
}
