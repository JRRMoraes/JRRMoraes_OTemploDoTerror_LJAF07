namespace Assets.Scripts.Tipos {

    [System.Serializable]
    public class Item {

        public string idItem;

        public int quantidade;


        public Item() { }


        public Item(string idItem, int quantidade) {
            this.idItem = idItem;
            this.quantidade = quantidade;
        }
    }
}