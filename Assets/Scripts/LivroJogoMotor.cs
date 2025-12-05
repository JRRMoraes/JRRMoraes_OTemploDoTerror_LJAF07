using Assets.Scripts.Componentes;
using Assets.Scripts.Tipos;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static Assets.Scripts.Tipos.Conjuntos;


namespace Assets.Scripts {

    public class LivroJogoMotor : MonoBehaviour, IPadraoObservador {

        public VISUALIZACAO visualizacao = VISUALIZACAO.TUDO;

        public BookPageCurlMotor bookPageCurlMotor { get; set; }

        public UIDocument uiDocument_PaginaDireitaCampanha;

        public UIDocument uiDocument_PaginaEsquerdaPanilha;

        public new Camera camera;

        public float historiaVelocidadeNormalDoTexto = Constantes.HISTORIA_VELOCIDADE_TEXTO_NORMAL;

        public float historiaVelocidadeDoTexto { get; set; } = Constantes.HISTORIA_VELOCIDADE_TEXTO_NORMAL;



        void Awake() {
            LivroJogo.INSTANCIA.observadoresAlvos.Inscrever(this);
            bookPageCurlMotor = GetComponent<BookPageCurlMotor>();
        }


        void Start() {
            LivroJogo.INSTANCIA.observadoresAlvos.Notificar(null);
        }


        void Update() {
            EntradaVisualizacaoDesktop();
        }


        void OnDestroy() {
            if (LivroJogo.INSTANCIA != null)
                LivroJogo.INSTANCIA.observadoresAlvos.Desinscrever(this);
        }


        public VisualElement Raiz_PaginaDireitaCampanha() {
            if (uiDocument_PaginaDireitaCampanha is null)
                return null;
            return uiDocument_PaginaDireitaCampanha.rootVisualElement;
        }


        public VisualElement Raiz_PaginaEsquerdaPanilha() {
            if (uiDocument_PaginaEsquerdaPanilha is null)
                return null;
            return uiDocument_PaginaEsquerdaPanilha.rootVisualElement;
        }


        public static bool EhValido(LivroJogoMotor livroJogoMotor, bool analisaRaiz = true) {
            if (livroJogoMotor is null)
                return false;
            if (analisaRaiz) {
                if ((livroJogoMotor.uiDocument_PaginaDireitaCampanha is null) || (livroJogoMotor.uiDocument_PaginaEsquerdaPanilha is null))
                    return false;
            }
            return true;
        }





        void EntradaVisualizacaoDesktop() {
            if ((Input.GetKeyDown(KeyCode.Q)) && (visualizacao != VISUALIZACAO.PANILHA)) {
                visualizacao = VISUALIZACAO.PANILHA;
                LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.VISUALIZACAO);
            }
            else if ((Input.GetKeyDown(KeyCode.W)) && (visualizacao != VISUALIZACAO.TUDO)) {
                visualizacao = VISUALIZACAO.TUDO;
                LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.VISUALIZACAO);
            }
            else if ((Input.GetKeyDown(KeyCode.E)) && (visualizacao != VISUALIZACAO.CAMPANHA)) {
                visualizacao = VISUALIZACAO.CAMPANHA;
                LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.VISUALIZACAO);
            }
            if (camera != null) {
                if (Input.GetKey(KeyCode.UpArrow))
                    camera.transform.position += new Vector3(0, 1, 0);
                else if (Input.GetKey(KeyCode.DownArrow))
                    camera.transform.position += new Vector3(0, -1, 0);
            }
        }


        public void AoNotificar(OBSERVADOR_CONDICAO observadorCondicao) {
            if (!OBSERVADOR_CONDICAO__JogoAtualEPaginaExecutora.Contains(observadorCondicao))
                return;
            if (AoNotificar_ObterPaginaExecutoraViaJogoAtual())
                return;
            if (AoNotificar_ProcessarPaginaExecutora())
                return;
        }


        public bool AoNotificar_ObterPaginaExecutoraViaJogoAtual() {
            if (PaginaExecutora.EhValido(LivroJogo.INSTANCIA.paginaExecutora))
                return false;
            if (!Livro.EhValido(LivroJogo.INSTANCIA.livro)) {
                bookPageCurlMotor.ImporPaginaAtual(0);
                return false;
            }
            if (!Jogo.EhValido(LivroJogo.INSTANCIA.jogoAtual, false)) {
                bookPageCurlMotor.ImporPaginaAtual(0);
                return false;
            }
            Pagina _paginaAtual = LivroJogo.INSTANCIA.ObterPaginaAtualViaJogoAtual();
            if (!Pagina.EhValido(_paginaAtual))
                return false;
            LivroJogo.INSTANCIA.paginaExecutora = new PaginaExecutora(_paginaAtual);
            Raiz_PaginaDireitaCampanha().dataSource = LivroJogo.INSTANCIA.paginaExecutora;
            Raiz_PaginaEsquerdaPanilha().dataSource = LivroJogo.INSTANCIA.jogoAtual.panilha;
            //IrParaOTopoDaPaginaViaScroll(); /////  window.scrollTo({ top: 0, behavior: "smooth" });
            LivroJogo.INSTANCIA.observadoresAlvos.Notificar(null);
            return true;
        }


        public bool AoNotificar_ProcessarPaginaExecutora() {
            if (!PaginaExecutora.EhValido(LivroJogo.INSTANCIA.paginaExecutora))
                return false;
            if (!Livro.EhValido(LivroJogo.INSTANCIA.livro))
                return false;
            if (!Jogo.EhValido(LivroJogo.INSTANCIA.jogoAtual, false))
                return false;
            switch (LivroJogo.INSTANCIA.paginaExecutora.paginaEstado) {
                case PAGINA_EXECUTOR_ESTADO.NULO:
                    LivroJogo.INSTANCIA.paginaExecutora.paginaEstado = PAGINA_EXECUTOR_ESTADO.INICIALIZADO;
                    LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                    return true;
                case PAGINA_EXECUTOR_ESTADO.INICIALIZADO:
                    LivroJogo.INSTANCIA.paginaExecutora.paginaEstado = PAGINA_EXECUTOR_ESTADO.HISTORIAS;
                    LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                    return true;
            }
            return false;
        }
    }
}