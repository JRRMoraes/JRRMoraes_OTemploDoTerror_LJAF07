using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;


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
                array = new T[] { };
            if (item == null)
                return array;
            T[] novoArray = new T[array.Length + 1];
            for (int i = 0; i < array.Length; i++)
                novoArray[i] = array[i];
            novoArray[novoArray.Length - 1] = item;
            return novoArray;
        }


        public static T Clonar<T>(T objetoOriginal) {
            string _json = JsonConvert.SerializeObject(objetoOriginal);
            return JsonConvert.DeserializeObject<T>(_json);
        }


        public static Visibility ObterVisibility(bool ehVisivel) {
            return (ehVisivel) ? Visibility.Visible : Visibility.Hidden;
        }


        public static DisplayStyle ObterDisplayStyle(bool ehVisivel) {
            return (ehVisivel) ? DisplayStyle.Flex : DisplayStyle.None;
        }


        public static async Task<Texture2D> CarregarImagemAsync(string caminho) {
            byte[] _dados;
#if UNITY_ANDROID && !UNITY_EDITOR
        //// Android requer WWW para acessar StreamingAssets
        var www = new WWW(caminho);
        await Task.Run(() => { while (!www.isDone) { } });
        dados = www.bytes;
#else
            _dados = await File.ReadAllBytesAsync(caminho);
#endif
            Texture2D _textura = new Texture2D(2, 2);
            if (_textura.LoadImage(_dados))
                return _textura;
            UnityEngine.Debug.LogError($"Falha ao carregar imagem '{caminho}'.");
            return null;
        }
    }
}