﻿using System;

namespace Tests.Diagnostics
{
    public class TooManyParameters : If
    {
        public TooManyParameters(int p1, int p2, int p3) { }
        public TooManyParameters(int p1, int p2, int p3, int p4) { } // Noncompliant
//                              ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

        public void F1(int p1, int p2, int p3) { }

        public void F2(int p1, int p2, int p3, int p4) { } // Compliant, interface implementation

        public void F1(int p1, int p2, int p3, int p4) { } // Noncompliant {{Method has 4 parameters, which is greater than the 3 authorized.}}

        public void F()
        {
            var v1 = new Action<int, int, int>(delegate(int p1, int p2, int p3) { Console.WriteLine(); });
            var v2 = new Action<int, int, int, int>(delegate(int p1, int p2, int p3, int p4) { Console.WriteLine(); }); // Noncompliant
            var v3 = new Action(delegate { });
            var v4 = new Action<int, int, int>((int p1, int p2, int p3) => Console.WriteLine());
            var v5 = new Action<int, int, int, int>((int p1, int p2, int p3, int p4) => Console.WriteLine()); // Noncompliant
            var v6 = new Action<object, object, object>((p1, p2, p3) => Console.WriteLine());
            var v7 = new Action<object, object, object, object>((p1, p2, p3, p4) => Console.WriteLine()); // Noncompliant
        }
    }

    public interface If
    {
        void F1(int p1, int p2, int p3);
        void F2(int p1, int p2, int p3, int p4); // Noncompliant
    }

    public class MyWrongClass
    {
        public MyWrongClass(string a, string b, string c, string d, string e, string f, string g, string h) // Noncompliant
        {
        }
    }

    public class SubClass : MyWrongClass
    {
        // See https://github.com/SonarSource/sonar-csharp/issues/1015
        // We should not raise when parent base class forces usage of too many args
        public SubClass(string a, string b, string c, string d, string e, string f, string g, string h) // Compliant (base class requires them)
            : base(a, b, c, d, e, f, g, h)
        {
        }

        public SubClass()
            : base("", "", "", "", "", "", "", "")
        { }
    }

    public class SubClass2 : TooManyParameters
    {
        public SubClass2(int p1, int p2, int p3, string s1, string s2) // Noncompliant
        {

        }
    }
}
