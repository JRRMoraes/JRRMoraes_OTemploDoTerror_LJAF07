using Assets.Scripts.Tipos;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;


namespace Assets.Scripts {

    public class LivroJogoMotor : MonoBehaviour {

        public Conjuntos.VISUALIZACAO_DESKTOP visualizacaoDesktop = Conjuntos.VISUALIZACAO_DESKTOP.TUDO;

        public Book book;

        public AutoFlip autoFlip;

        public UIDocument uiDocument;

        public VisualElement raiz { get; set; }

        public VisualElement paginaPanilha;

        public VisualElement panilhaNova;

        public VisualElement panilhaCompleta;

        public VisualElement panilhaMenor;

        public VisualElement paginaCampanha;

        public VisualElement paginaTitulo;

        public VisualElement historia;

        public VisualElement combate;

        public VisualElement destino;


        void Awake() {
            raiz = uiDocument.rootVisualElement;
            if (raiz is null)
                return;
            /////
            ///// teste
            if (LivroJogo.INSTANCIA.jogoAtual is null) {
                LivroJogo.INSTANCIA.jogoAtual = LivroJogo.INSTANCIA.jogoSalvo_3.Clonar();
                LivroJogo.INSTANCIA.jogoAtual.AjustarSeForNovoJogo();
                if (LivroJogo.INSTANCIA.jogoAtual.campanhaPagina == 1) {
                    LivroJogo.INSTANCIA.jogoAtual.campanhaPagina = 3;
                    LivroJogo.INSTANCIA.jogoAtual.panilha = Panilha.CriarPanilhaViaRolagens(new DadosRoladosTotaisParaPanilhaNova() { habilidade = 10, energia = 14, sorte = 9 }, "TtTtT", Conjuntos.JOGO_NIVEL.FACIL);
                    LivroJogo.INSTANCIA.paginaAtual = LivroJogo.INSTANCIA.livro.paginasCampanha
                        .FirstOrDefault<Pagina>(paginaI => ((paginaI.idCapitulo == Conjuntos.CAMPANHA_CAPITULO.PAGINAS_INICIAIS) && (paginaI.idPagina == 3)));
                }
            }
            /////
            /////
            raiz.dataSource = LivroJogo.INSTANCIA;
            paginaPanilha = raiz.Query<VisualElement>("PaginaPanilha");
            panilhaNova = raiz.Query<VisualElement>("PanilhaNova");
            panilhaCompleta = raiz.Query<VisualElement>("PanilhaCompleta");
            panilhaMenor = raiz.Query<VisualElement>("PanilhaMenor");
            paginaCampanha = raiz.Query<VisualElement>("PaginaCampanha");
            paginaTitulo = raiz.Query<VisualElement>("PaginaTitulo");
            historia = raiz.Query<VisualElement>("Historia");
            combate = raiz.Query<VisualElement>("Combate");
            destino = raiz.Query<VisualElement>("Destino");
        }


        void Start() {
            AlterarVisualizacaoDesktop();
        }


        void Update() {
            EntradaVisualizacaoDesktop();
        }


        public void PassarPaginasADireita() {
            if ((book is null) || (autoFlip is null))
                return;
            if ((book.currentPage + 2) <= book.TotalPageCount)
                autoFlip.FlipRightPage();
            else
                autoFlip.FlipLeftPage();
        }


        public void PassarPaginasAEsquerda() {
            if ((book is null) || (autoFlip is null))
                return;
            if ((book.currentPage - 2) >= 0)
                autoFlip.FlipLeftPage();
            else
                autoFlip.FlipRightPage();
        }


        void EntradaVisualizacaoDesktop() {
            if ((Input.GetKeyDown(KeyCode.Q)) && (visualizacaoDesktop != Conjuntos.VISUALIZACAO_DESKTOP.PANILHA)) {
                visualizacaoDesktop = Conjuntos.VISUALIZACAO_DESKTOP.PANILHA;
                AlterarVisualizacaoDesktop();
            }
            else if ((Input.GetKeyDown(KeyCode.W)) && (visualizacaoDesktop != Conjuntos.VISUALIZACAO_DESKTOP.TUDO)) {
                visualizacaoDesktop = Conjuntos.VISUALIZACAO_DESKTOP.TUDO;
                AlterarVisualizacaoDesktop();
            }
            else if ((Input.GetKeyDown(KeyCode.E)) && (visualizacaoDesktop != Conjuntos.VISUALIZACAO_DESKTOP.CAMPANHA)) {
                visualizacaoDesktop = Conjuntos.VISUALIZACAO_DESKTOP.CAMPANHA;
                AlterarVisualizacaoDesktop();
            }
        }


        void AlterarVisualizacaoDesktop() {
            if ((paginaPanilha is null) || (paginaCampanha is null))
                return;
            bool _ehPanilhaNova = (LivroJogo.INSTANCIA.paginaAtual.idCapitulo == Conjuntos.CAMPANHA_CAPITULO.PAGINAS_INICIAIS)
                && (LivroJogo.INSTANCIA.paginaAtual.idPagina == 1);
            if (visualizacaoDesktop == Conjuntos.VISUALIZACAO_DESKTOP.PANILHA) {
                panilhaNova.SetEnabled(_ehPanilhaNova);
                panilhaCompleta.SetEnabled((!_ehPanilhaNova) && (true));
                panilhaMenor.SetEnabled((!_ehPanilhaNova) && (false));
                paginaPanilha.style.width = new Length(75, LengthUnit.Percent);
                paginaCampanha.style.width = new Length(25, LengthUnit.Percent);
            }
            else if (visualizacaoDesktop == Conjuntos.VISUALIZACAO_DESKTOP.TUDO) {
                panilhaNova.SetEnabled(_ehPanilhaNova);
                panilhaCompleta.SetEnabled((!_ehPanilhaNova) && (true));
                panilhaMenor.SetEnabled((!_ehPanilhaNova) && (false));
                paginaPanilha.style.width = new Length(50, LengthUnit.Percent);
                paginaCampanha.style.width = new Length(50, LengthUnit.Percent);
            }
            else if (visualizacaoDesktop == Conjuntos.VISUALIZACAO_DESKTOP.CAMPANHA) {
                panilhaNova.SetEnabled(_ehPanilhaNova);
                panilhaCompleta.SetEnabled((!_ehPanilhaNova) && (false));
                panilhaMenor.SetEnabled((!_ehPanilhaNova) && (true));
                paginaPanilha.style.width = new Length(25, LengthUnit.Percent);
                paginaCampanha.style.width = new Length(75, LengthUnit.Percent);
            }
        }
    }
}
