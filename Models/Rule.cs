namespace AvaloniaTuring.Models
{
    public class Rule : TuringObject
    {

        public int nextState;
        public int currentState;
        public char currentSymbol;
        public char symbolToWrite;
        public TuringDirection currDirection;
        public bool isRule;

        public Rule()
        {
            ID = GlobalData.Instance.gTuringObjectNum;
            GlobalData.Instance.gTuringObjectNum++;
            isRule = false;
        }

        public override int GetID()
        {
            return ID;
        }

    }
}
