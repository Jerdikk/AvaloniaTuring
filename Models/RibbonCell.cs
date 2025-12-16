namespace AvaloniaTuring.Models
{
    public class RibbonCell : TuringObject
    {
        private char _RibbonSymbol;
        public char RibbonSymbol
        {
            get { return _RibbonSymbol; }
            set { _RibbonSymbol = value; }
        }

        public RibbonCell(char t1)
        {
            ID = GlobalData.Instance.gTuringObjectNum;
            GlobalData.Instance.gTuringObjectNum++;
            Set(t1);
        }

        public RibbonCell()
        {
            ID = GlobalData.Instance.gTuringObjectNum;
            GlobalData.Instance.gTuringObjectNum++;
        }

        public void Set(char value)
        {
            RibbonSymbol = value;
        }

        public char Get()
        {
            return RibbonSymbol;
        }

        public override int GetID()
        {
            return ID;
        }
    }
}
