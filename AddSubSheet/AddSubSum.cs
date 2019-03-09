using System;
using System.Collections.Generic;

namespace AddSubSheet
{
    enum Operator
    {
        Add, Sub
    }


    class SimpleSum
    {
        static Random rnd = new Random();

        public SimpleSum(int type)
        {
            switch (type)
            {
                case 0:
                    NewAdd();
                    break;
                case 1:
                    NewSubNoBorrow();
                    break;
                case 2:
                    NewSubWithBorrow();
                    break;
            }
        }

        public int First { get; set; }
        public int Second { get; set; }
        public int Answer { get; set; }
        public Operator op { get; set; }


        public void NewSubNoBorrow()
        {
            First = rnd.Next(10, 100);
            Second = rnd.Next(First) / 10 * 10;
            var units = rnd.Next(0, First % 10);
            Second += units;
            op = Operator.Sub;
            Answer = First - Second;
            if (Answer < 0)
            {
                Console.WriteLine();
            }
        }
        public void NewSubWithBorrow()
        {
            First = rnd.Next(10, 100);
            Second = rnd.Next((First / 10 - 1) * 10);
            var units = rnd.Next(First % 10 + 1, 10);
            Second += units;
            op = Operator.Sub;
            Answer = First - Second;
            if (Answer < 0)
            {
                Console.WriteLine();
            }
        }

        public void NewAdd()
        {
            First = rnd.Next(100);
            Second = rnd.Next(100);
            op = Operator.Add;
            Answer = First + Second;
        }
    }

    internal class AddSubSum
    {
        int _sumsPerPage;

        public AddSubSum(int sumsPerPage)
        {
            this._sumsPerPage = sumsPerPage;
        }

        private IEnumerable<SimpleSum> Generator()
        {
            var rnd = new Random();
            var sumlist = new List<SimpleSum>();
            while (true)
            {
                var l1 = _sumsPerPage / 2;
                var l2 = _sumsPerPage * 3 / 4;
                for (int i = 0; i < l1; i++)
                {
                    sumlist.Add(new SimpleSum(0));
                }
                for (int i = l1; i < l2; i++)
                {
                    sumlist.Add(new SimpleSum(1));
                }
                for (int i = l2; i < _sumsPerPage; i++)
                {
                    sumlist.Add(new SimpleSum(2));
                }

                // randomly shuffle the list:
                for (int i = sumlist.Count - 1; i > 0; i--)
                {
                    // random from zero to i:
                    var j = rnd.Next(i + 1);

                    // swap:
                    var temp = sumlist[i];
                    sumlist[i] = sumlist[j];
                    sumlist[j] = temp;
                }

                foreach (var l in sumlist)
                {
                    yield return l;
                }
            }
        }

        public IEnumerable<(int first, int second, int answer, char op)> GetEnumerable()
        {
            foreach (var sum in Generator())
            {
                char c = sum.op == Operator.Add ? '+' : '-';
                yield return (sum.First, sum.Second, sum.Answer, c);
            }
        }
    }
}