namespace PayWithPlay.Core.ViewModels
{
    public abstract class DialogInputViewModel : BaseViewModel<Tuple<string, Action<string>>>
    {
        private Action<string>? _onDoneAction;
        private string? _value;

        public override void ViewDestroy(bool viewFinishing = true)
        {
            _onDoneAction = null;

            base.ViewDestroy(viewFinishing);
        }

        public enum InputTypeEnum { Text, Numeric, Decimal }

        public abstract InputTypeEnum InputType { get; }

        public virtual string Prefix => string.Empty;

        public abstract string Title { get; }

        public abstract string HintText { get; }

        public abstract string DoneButtonText { get; }

        public string? Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        public virtual void OnDone()
        {
            NavigationService.Close(this);
            _onDoneAction?.Invoke(Value);
        }

        public virtual void OnClose()
        {
            NavigationService.Close(this);
        }

        public override void Prepare(Tuple<string, Action<string>> parameter)
        {
            Value = parameter.Item1;
            _onDoneAction = parameter.Item2;
        }
    }
}
