using System.Collections.Generic;

namespace HomeAssistantTaskbarWidget.Model.HA
{
    public class State
    {
        private string _state;

        public IDictionary<string, string> Mapping { get; set; }

        public State(string value)
        {
            _state = value;
        }

        public static implicit operator State(string value) => new State(value);

        public override string ToString()
        {
            return _state;
        }

        public string Map()
        {
            if (Mapping == null || Mapping.Count == 0)
                return _state;

            if (Mapping.TryGetValue(_state, out var mappedState))
                return mappedState;

            return _state;
        }
    }
}
