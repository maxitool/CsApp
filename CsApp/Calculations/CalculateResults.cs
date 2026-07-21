using Values = CsApp.DB.Models.Values;
using Results = CsApp.DB.Models.Results;
using System.Collections;

namespace CsApp.Calculations
{
    public class CalculateResults
    {
        public DateTime? MaxDateTime { get { return _maxDateTime; } }
        public DateTime? MinDateTime { get { return _minDateTime; }}
        public TimeSpan? DeltaDateTimes { get { return _maxDateTime - _minDateTime; } }
        public List<decimal> ValuesList { get { return new List<decimal>(_valuesList); } }
        public int CountElements { get { return _countElements; } }
        public decimal AvgExecutionTime 
        { 
            get 
            {
                if (_countElements <= 0)
                    return 0;
                return (decimal)_allExecutionTime / (decimal)_countElements; 
            } 
        }
        public decimal AvgValue
        {
            get
            {
                if (_countElements <= 0)
                    return 0;
                decimal result = 0;
                foreach (decimal item in ValuesList)
                    result += item;
                return result / (decimal)_countElements;
            }
        }
        public decimal MedianValue
        {
            get
            {
                if (_countElements <= 0)
                    return 0;
                if (_countElements % 2 == 1)
                    return _valuesList[_valuesList.Count / 2];
                int pos = _valuesList.Count / 2;
                return (_valuesList[pos - 1] + _valuesList[pos]) / 2;
            }
        }
        public decimal MinValue { get { return _minValue; } }
        public decimal MaxValue { get { return _maxValue; } }

        protected DateTime? _maxDateTime;
        protected DateTime? _minDateTime;
        protected List<decimal> _valuesList = new List<decimal>();
        protected decimal _allExecutionTime;
        protected int _countElements;
        protected decimal _minValue;
        protected decimal _maxValue;


        public void AddValue(Values value)
        {
            if (_countElements <= 0)
            {
                _maxDateTime = value.date;
                _minDateTime = value.date;
                _maxValue = value.value;
                _minValue = value.value;
            }
            else
            {
                if (_maxDateTime < value.date)
                    _maxDateTime = value.date;
                else if (_minDateTime > value.date)
                    _minDateTime = value.date;
                if (_maxValue < value.value)
                    _maxValue = value.value;
                else if (_minValue > value.value)
                    _minValue = value.value;
            }
            _countElements += 1;
            _allExecutionTime += value.execution_time;
            AddSorted(value.value);
        }

        public Results? CalculateResultsFromValue(int id_file)
        {
            Results result = new();
            result.id_file = id_file;
            if (_countElements <= 0)
            {
                Console.WriteLine($"No elements in {nameof(CalculateResults)} class.");
                return null;
            }
            result.delta_date = (TimeSpan)DeltaDateTimes;
            result.min_date = (DateTime)_minDateTime;
            result.avg_execution_time = AvgExecutionTime;
            result.avg_value = AvgValue;
            result.median_value = MedianValue;
            result.max_value = _maxValue;
            result.min_value = _minValue;
            return result;
        }

        protected void AddSorted(decimal item)
        {
            if (_valuesList.Count == 0)
            {
                _valuesList.Add(item);
                return;
            }
            int index = _valuesList.BinarySearch(item);

            if (index < 0)
                index = ~index; // Convert to the correct insertion index
            _valuesList.Insert(index, item);
        }
    }
}
