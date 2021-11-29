using System;

interface IUserInteractions
{
    string GetPrompt();
    void HandleInput(string input);
    bool ShouldContinue();

}

enum State
{
    Init,
    Start,
    Process,
    Finish,
    Halt
}

namespace AssocsConsole
{
    public class UserInteractions: IUserInteractions
    {
        public UserInteractions()
        {
            _state = State.Init;
        }

        private IWordWorker _worker;
        private  State _state;

        public string GetPrompt()
        {
            if (_state == State.Init)
            {
                return "Чтобы начать искать скрытые ассоциации отправь любое сообщение \nЧтобы закончить и выйти отправь \"---\" в любой момент";
            }
            if (_state == State.Start)
            {
                return "С каким словом будем искать ассоциации?";
            }
            if (_state == State.Process)
            {
                var source = _worker.GetAssociationSource();
                if (source.Count == 1)
                {
                    return $"Что у тебя ассоциируется со словом \"{source[0]}\"";
                }
                return $"Что у тебя ассоциируется со словами \"{source[0]}\" и \"{source[1]}\"";
            }
            if (_state == State.Finish)
            {
                _state = State.Init;
                return $"Похоже, что твоя глубинная ассоциация со словом {_worker.MainWord} это {_worker.GetResult()}";
            }
            return "See ya!";
        }

        public bool ShouldContinue()
        {
            return _state == State.Halt;
        }

        public void HandleInput(string input)
        {
            if (input == "---")
            {
                _state = State.Halt;
                return;
            }
            if (_state == State.Init)
            {
                _state = State.Start;
                return;

            }
            if (_state == State.Start)
            {
                _worker = new WordWorker(input);
                _state = State.Process;
                return;
            }
            if (_state == State.Finish)
            {
                _state = State.Init;
            }

            _worker.AddAssoc(input);

            if (_worker.IsComplete())
            {
                _state = State.Finish;
            }

        }
    }
}
