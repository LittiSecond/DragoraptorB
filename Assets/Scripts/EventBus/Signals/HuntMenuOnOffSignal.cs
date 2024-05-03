namespace EventBus
{
    public class HuntMenuOnOffSignal
    {
        public bool IsOpened { get; private set; }

        public HuntMenuOnOffSignal(bool isOpened)
        {
            IsOpened = isOpened;
        }
    }
}