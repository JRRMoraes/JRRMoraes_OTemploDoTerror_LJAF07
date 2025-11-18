using Assets.Scripts.Tipos;
using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static Assets.Scripts.Tipos.Conjuntos;


namespace Assets.Scripts {

    public class LivroJogo : MonoBehaviour {

        public static LivroJogo INSTANCIA {
            get {
                if (_INSTANCIA == null) {
                    _INSTANCIA = FindFirstObjectByType<LivroJogo>();
                    //if (INSTANCIA == null) {
                    //    GameObject lObjeto = new GameObject();
                    //    lObjeto.name = typeof(LivroJogo).Name;
                    //    INSTANCIA = lObjeto.AddComponent<LivroJogo>();
                    //}
                }
                return _INSTANCIA;
            }
        }
        private static LivroJogo _INSTANCIA = null;

        [Header("LIVRO")]

        public Livro livro = null;

        [Header("Arquivo JSON (Assets/StreamingAssets/*.json)")]
        public string arquivoJsonDoLivro;

        [Header("JOGO")]

        public Jogo jogoSalvo_1 = new Jogo(1);

        public Jogo jogoSalvo_2 = new Jogo(2);

        public Jogo jogoSalvo_3 = new Jogo(3);

        public Jogo jogoAtual = null;

        public bool ehJogoCarregado;

        public EfeitoExecucao[] jogadorEfeitosAplicados { get; set; }

        [Header("PÁGINA")]

        public PaginaExecutora paginaExecutora = null;

        ArquivadorDeJogo _arquivadorDeJogo;

        public PadraoObservadorAlvo observadoresAlvos = new PadraoObservadorAlvo();

        [Header("CONSTANTES")]

        public static string IMAGEM_CAMINHO_LIVRO_JOGO = Path.Combine("LJAF07_OTemploDoTerror", "Imagens");

        public static string IMAGEM_EXTENSAO = ".png";



        void Awake() {
            if ((INSTANCIA != null) && (INSTANCIA != this)) {
                Destroy(gameObject);
                return;
            }
            _INSTANCIA = this;
            observadoresAlvos.monoBehaviour = _INSTANCIA;
            DontDestroyOnLoad(gameObject);
        }


        void Start() {
            CarregarLivro();
            CarregarJogosSalvos();
            observadoresAlvos.Notificar(null);
        }


        void CarregarLivro() {
            LeitorJsonResultaLivro _leitor = GetComponent<LeitorJsonResultaLivro>();
            livro = _leitor.LerJsonResultandoLivro(arquivoJsonDoLivro);
        }


        void CarregarJogosSalvos() {
            ResetarJogo();
            _arquivadorDeJogo = GetComponent<ArquivadorDeJogo>();
            jogoSalvo_1 = _arquivadorDeJogo.CarregarJogo(jogoSalvo_1.idJogo);
            jogoSalvo_2 = _arquivadorDeJogo.CarregarJogo(jogoSalvo_2.idJogo);
            jogoSalvo_3 = _arquivadorDeJogo.CarregarJogo(jogoSalvo_3.idJogo);
            /////
            ///// teste
            if (!Jogo.EhValido(jogoAtual)) {
                jogoAtual = LivroJogo.INSTANCIA.jogoSalvo_3.Clonar();
                jogoAtual.AjustarSeForNovoJogo();
                if (jogoAtual.campanhaIdPagina == 1) {
                    jogoAtual.campanhaIdPagina = 2;
                }
                if (!Panilha.EhValido(jogoAtual.panilha))
                    jogoAtual.panilha = Panilha.CriarPanilhaViaRolagens(new DadosRoladosTotaisParaPanilhaNova() { habilidade = 10, energia = 14, sorte = 9 }, "TtTtT", JOGO_NIVEL.FACIL);
            }
            ///// teste
            /////
        }


        public void ResetarJogo() {
            jogoAtual = null;
            paginaExecutora = null;
            //setJogadorEfeitosAplicados([]);
            //setPadraoCapitulo(ECampanhaCapitulo.PAGINAS_INICIAIS);
            observadoresAlvos.Notificar(null);
        }


        public void SalvarJogoAtualNoJogoSalvo() {
            if (_arquivadorDeJogo is null)
                return;
            if (!Jogo.EhValido(jogoAtual))
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
            ehJogoCarregado = jogoAtual.AjustarSeForNovoJogo();
            SceneManager.LoadScene("LivroJogo", LoadSceneMode.Single);
        }


        public void SelecionarJogoSalvo_2(ClickEvent evento) {
            jogoAtual = jogoSalvo_2.Clonar();
            ehJogoCarregado = jogoAtual.AjustarSeForNovoJogo();
            SceneManager.LoadScene("LivroJogo", LoadSceneMode.Single);
        }


        public void SelecionarJogoSalvo_3(ClickEvent evento) {
            jogoAtual = jogoSalvo_3.Clonar();
            ehJogoCarregado = jogoAtual.AjustarSeForNovoJogo();
            SceneManager.LoadScene("LivroJogo", LoadSceneMode.Single);
        }


        public void ImporCampanhaDestinoNoJogoAtual(int idPaginaDestino, CAMPANHA_CAPITULO idCapituloDestino) {
            if ((jogoAtual.campanhaIdPagina == idPaginaDestino) && (jogoAtual.campanhaIdCapitulo == idCapituloDestino))
                return;
            jogoAtual.campanhaIdPagina = idPaginaDestino;
            jogoAtual.campanhaIdCapitulo = idCapituloDestino;
            paginaExecutora = null;
        }


        public static string MontarArquivoECaminho(string caminho, string arquivo) {
            string _arquivo = Path.Combine(Application.streamingAssetsPath, caminho, arquivo);
            return _arquivo;
        }


        public Pagina ObterPaginaAtualViaJogoAtual() {
            if ((!Livro.EhValido(LivroJogo.INSTANCIA.livro)) || (!Jogo.EhValido(jogoAtual)))
                return PaginaUtils.PAGINA_ZERADA();
            if (jogoAtual.campanhaIdCapitulo == CAMPANHA_CAPITULO.NULO) {
                jogoAtual.campanhaIdPagina = PaginaUtils.PAGINA_INICIAL().idPagina;
                jogoAtual.campanhaIdCapitulo = PaginaUtils.PAGINA_INICIAL().idCapitulo;
            }
            Pagina _paginaAtual = null;
            if (jogoAtual.campanhaIdCapitulo == CAMPANHA_CAPITULO.PAGINAS_INICIAIS)
                _paginaAtual = livro.paginasIniciais.FirstOrDefault<Pagina>(_paginaI => _paginaI.idPagina == jogoAtual.campanhaIdPagina);
            else if (jogoAtual.campanhaIdCapitulo == CAMPANHA_CAPITULO.PAGINAS_CAMPANHA)
                _paginaAtual = livro.paginasCampanha.FirstOrDefault<Pagina>(_paginaI => _paginaI.idPagina == jogoAtual.campanhaIdPagina);
            if (_paginaAtual is null) {
                Console.WriteLine("ContextoLivro.ObterPagina:::  Não foi possível encontrar a página " + jogoAtual.campanhaIdPagina + " da " + jogoAtual.campanhaIdCapitulo);
                return PaginaUtils.PAGINA_ZERADA();
            }
            if (_paginaAtual.idCapitulo == CAMPANHA_CAPITULO.NULO)
                _paginaAtual.idCapitulo = jogoAtual.campanhaIdCapitulo;
            return _paginaAtual;
        }


        public void AdicionarEmJogadorEfeitosAplicados(EfeitoExecucao[] efeitosExecucao) {
            foreach (EfeitoExecucao _efeitoI in efeitosExecucao) {
                if (jogadorEfeitosAplicados is null)
                    Uteis.AdicionarNoArray<EfeitoExecucao>(jogadorEfeitosAplicados, _efeitoI);
                else if (!jogadorEfeitosAplicados.Any((_efeitoI2) => _efeitoI2.exeIdEfeito == _efeitoI.exeIdEfeito))
                    Uteis.AdicionarNoArray<EfeitoExecucao>(jogadorEfeitosAplicados, _efeitoI);
            }
        }

        public bool EhFimDeJogo() {
            bool _estaMorto = (jogoAtual.panilha != null)
                && (jogoAtual.panilha.energiaInicial >= 1)
                && (jogoAtual.panilha.energia <= 0);
            bool _destinoMorte = (paginaExecutora != null)
                && (paginaExecutora.destinoItens != null)
                && (paginaExecutora.destinoItens.Any((_destinoI) => ((_destinoI.idPagina == PaginaUtils.PAGINA_FIM_DE_JOGO().idPagina) && (_destinoI.idCapitulo == PaginaUtils.PAGINA_FIM_DE_JOGO().idCapitulo))));
            bool _resultadoDerrota = (paginaExecutora.combateResultadoFinalDerrota == RESULTADO_COMBATE.DERROTA)
                || (paginaExecutora.combateResultadoFinalInimigos == RESULTADO_COMBATE.DERROTA);
            return (_estaMorto || _destinoMorte || _resultadoDerrota);
        }
    }
}
