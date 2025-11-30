using Assets.Scripts.Tipos;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;


namespace Assets.Scripts {

    public class LeitorJsonResultaLivro : MonoBehaviour {

        public Livro LerJsonResultandoLivro(string pArquivoJsonDoLivro) {
            Livro _livro = new Livro();
            try {
                string _arquivo = Path.Combine(Application.streamingAssetsPath, pArquivoJsonDoLivro);
                if (!File.Exists(_arquivo)) {
                    Debug.LogError($"Arquivo JSON não encontrado: {_arquivo}");
                    return _livro;
                }
                string _json = File.ReadAllText(_arquivo);
                Debug.Log($"_json = {_json}");
                _livro = JsonConvert.DeserializeObject<Livro>(_json);
                Debug.Log($"Livro = {JsonConvert.SerializeObject(_livro)}");
            }
            catch (System.Exception ex) {
                Debug.LogError($"Erro ao ler JSON: {ex.Message}");
            }
            return _livro;
        }
    }
}