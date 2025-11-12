using UnityEngine;


namespace Assets.Scripts {

    public class Uteis {

        public static bool TextosIguais(string texto1, string texto2, bool ehCaseSensitive = false) {
            if (string.IsNullOrEmpty(texto1))
                texto1 = "";
            if (string.IsNullOrEmpty(texto2))
                texto2 = "";
            if (ehCaseSensitive)
                return texto1 == texto2;
            else
                return texto1.ToUpper() == texto2.ToUpper();
        }


        public static T[] AdicionarNoArray<T>(T[] array, T item) {
            if (array == null)
                array = new T[0];
            if (item == null)
                return array;
            T[] novoArray = new T[array.Length + 1];
            for (int i = 0; i < array.Length; i++)
                novoArray[i] = array[i];
            novoArray[novoArray.Length - 1] = item;
            return novoArray;
        }


        public static T Clonar<T>(T objetoOriginal) {
            string _json = JsonUtility.ToJson(objetoOriginal);
            return JsonUtility.FromJson<T>(_json);
        }
    }
}