﻿using System;
using System.Collections.Generic;

namespace AddSubSheet
{
    class Counters
    {
        public int Adds { get; set; } = 0;
        public int Subs { get; set; } = 0;
        public int SubsWithBorrow { get; set; } = 0;
        public int Same { get; set; } = 0;
    }
    internal class AddSubSum
    {
        int _sumsPerPage;
        static Counters counters = new Counters();

        readonly Random rnd = new Random();
        int _firstOperand;
        int _secondOperand;
        Operator _operator;

        enum Operator
        {
            Add, Sub
        }
        public AddSubSum(int sumsPerPage)
        {
            this._sumsPerPage = sumsPerPage;
        }

        private IEnumerable<AddSubSum> Generator()
        {
            var sumlist = new List<AddSubSum>();
            while (true)
            {
                var l1 = _sumsPerPage / 2;
                var l2 = _sumsPerPage * 3 / 4;
                for (int i = 0; i < l1; i++)
                {
                    var sum = GenerateAddSum();
                }
                for (int i = l1; i < l2; i++)
                {
                    var sum = GenerateSubSumNoBorrow();
                }
                for (int i = l2; i < _sumsPerPage; i++)
                {
                    var sum = GenerateSubSum();
                }
            }
        }

        private object GenerateSubSumNoBorrow()
        {
            throw new NotImplementedException();
        }

        private object GenerateAddSum()
        {
            throw new NotImplementedException();
        }

        private object GenerateSubSum()
        {
            throw new NotImplementedException();
        }

        public AddSubSum Next()
        {
            var diffCounters = counters.Adds - counters.Subs;
            if (Math.Abs(diffCounters) < 3)
            {
                var generator = rnd.Next(2);
                _operator = generator == 1 ? Operator.Add : Operator.Sub;
            }
            else if (diffCounters < 0)
            {
                _operator = Operator.Add;
            }
            else
            {
                _operator = Operator.Sub;
            }

            if (_operator == Operator.Add)
            {
                counters.Adds++;
            }
            else if (_operator == Operator.Sub)
            {
                counters.Subs++;
            }

            _firstOperand = rnd.Next(100);
            _secondOperand = rnd.Next(100);
            if (_operator == Operator.Sub && _firstOperand < _secondOperand)
            {
                var temp = _firstOperand;
                _firstOperand = _secondOperand;
                _secondOperand = temp;
            }
            return this;
        }

        public override string ToString()
        {
            char c = '@';
            switch (_operator)
            {
                case Operator.Add:
                    c = '+';
                    break;
                case Operator.Sub:
                    c = '-';
                    break;
            }
            return $"{_firstOperand} {c} {_secondOperand} = ";
        }

    }
}