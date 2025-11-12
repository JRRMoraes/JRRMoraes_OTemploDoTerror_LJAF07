using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Assets.Scripts.Tipos;


namespace Assets.Scripts {

    public class LivroJogo : MonoBehaviour {

        public static LivroJogo INSTANCIA {
            get {
                if (_INSTANCIA == null) {
                    _INSTANCIA = FindFirstObjectByType<LivroJogo>();
                    if (_INSTANCIA == null) {
                        GameObject lObjeto = new GameObject();
                        lObjeto.name = typeof(LivroJogo).Name;
                        _INSTANCIA = lObjeto.AddComponent<LivroJogo>();
                    }
                }
                return _INSTANCIA;
            }
        }
        private static LivroJogo _INSTANCIA = null;


        protected virtual void Awake() {
            if (_INSTANCIA == null) {
                _INSTANCIA = (this as LivroJogo);
                DontDestroyOnLoad(this.gameObject);
            }
            else {
                Destroy(gameObject);
            }
        }



        [Header("LIVRO")]

        public Livro livro;

        [Header("Arquivo JSON (Assets/StreamingAssets/*.json)")]
        public string arquivoJsonDoLivro;

        [Header("JOGO")]

        public Jogo jogoSalvo_1 = new Jogo(1);

        public Jogo jogoSalvo_2 = new Jogo(2);

        public Jogo jogoSalvo_3 = new Jogo(3);

        public Jogo jogoAtual;

        public Pagina paginaAtual;

        ArquivadorDeJogo _arquivadorDeJogo;



        void Start() {
            CarregarLivro();
            CarregarJogosSalvos();
        }


        void CarregarLivro() {
            LeitorJsonResultaLivro _leitor = GetComponent<LeitorJsonResultaLivro>();
            if (_leitor is null)
                return;
            livro = _leitor.LerJsonResultandoLivro(arquivoJsonDoLivro);
        }


        void CarregarJogosSalvos() {
            ResetarJogo();
            _arquivadorDeJogo = GetComponent<ArquivadorDeJogo>();
            if (_arquivadorDeJogo is null)
                return;
            jogoSalvo_1 = _arquivadorDeJogo.CarregarJogo(jogoSalvo_1.idJogo);
            jogoSalvo_2 = _arquivadorDeJogo.CarregarJogo(jogoSalvo_2.idJogo);
            jogoSalvo_3 = _arquivadorDeJogo.CarregarJogo(jogoSalvo_3.idJogo);
        }


        public void ResetarJogo() {
            jogoAtual = null;
            //setJogadorEfeitosAplicados([]);
            //setPadraoCapitulo(ECampanhaCapitulo.PAGINAS_INICIAIS);
        }


        public void SalvarJogoAtualNoJogoSalvo() {
            if (_arquivadorDeJogo is null)
                return;
            if (jogoAtual is null)
                return;
            jogoAtual.dataSalvo = new DateTime();
            if (jogoAtual.idJogo == 1) {
                jogoSalvo_1 = jogoAtual.Clonar();
                _arquivadorDeJogo.SalvarJogo(jogoSalvo_1);
            }
            else if (jogoAtual.idJogo == 2) {
                jogoSalvo_2 = jogoAtual.Clonar();
                _arquivadorDeJogo.SalvarJogo(jogoSalvo_2);
            }
            else if (jogoAtual.idJogo == 3) {
                jogoSalvo_3 = jogoAtual.Clonar();
                _arquivadorDeJogo.SalvarJogo(jogoSalvo_3);
            }
        }


        public void SelecionarJogoSalvo_1(ClickEvent evento) {
            jogoAtual = jogoSalvo_1.Clonar();
            jogoAtual.AjustarSeForNovoJogo();
            SceneManager.LoadScene("LivroJogo", LoadSceneMode.Single);
        }


        public void SelecionarJogoSalvo_2(ClickEvent evento) {
            jogoAtual = jogoSalvo_2.Clonar();
            jogoAtual.AjustarSeForNovoJogo();
            SceneManager.LoadScene("LivroJogo", LoadSceneMode.Single);
        }


        public void SelecionarJogoSalvo_3(ClickEvent evento) {
            jogoAtual = jogoSalvo_3.Clonar();
            jogoAtual.AjustarSeForNovoJogo();
            SceneManager.LoadScene("LivroJogo", LoadSceneMode.Single);
        }
    }
}
