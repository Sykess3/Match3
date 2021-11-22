namespace _Project.Code.Core.Models
{
    public class SwitchCommand
    {
        public Cell Cell1 { get; }
        public Cell Cell2 { get; }

        public SwitchCommand(Cell cell1, Cell cell2)
        {
            Cell1 = cell1;
            Cell2 = cell2;
        }

        public void Execute() => Swap();

        public void Revert() => Swap();

        private void Swap()
        {
            var fillerPosition = Cell1.Filler.Position;
            Cell1.Filler.Position = Cell2.Filler.Position;
            Cell2.Filler.Position = fillerPosition;
        }
    }
}