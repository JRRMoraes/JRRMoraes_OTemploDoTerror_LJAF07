using Assets.Scripts.Tipos;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static Assets.Scripts.Tipos.Conjuntos;


namespace Assets.Scripts.Componentes {

    [RequireComponent(typeof(LivroJogoMotor))]
    public class PaginaPanilha : MonoBehaviour, IPadraoObservador {

        LivroJogoMotor livroJogoMotor;

        VisualElement paginaPanilha;

        TabView panilhaTabView;

        Tab menuInicialTab;

        Tab panilhaNovaTab;

        Tab panilhaCompletaTab;

        Tab panilhaMenorTab;


        void Awake() {
            livroJogoMotor = GetComponent<LivroJogoMotor>();
            LivroJogo.INSTANCIA.observadoresAlvos.Inscrever(this);
        }


        public void AoNotificar(OBSERVADOR_CONDICAO observadorCondicao) {
            if (!LivroJogoMotor.EhValido(livroJogoMotor))
                return;
            if (!OBSERVADOR_CONDICAO__JogoAtualEPaginaExecutora.Contains(observadorCondicao))
                return;
            if (paginaPanilha is null)
                paginaPanilha = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<VisualElement>("PaginaPanilha");
            if (panilhaTabView is null)
                panilhaTabView = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<TabView>("PanilhaTabView");
            if (menuInicialTab is null)
                menuInicialTab = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<Tab>("MenuInicialTab");
            if (panilhaNovaTab is null)
                panilhaNovaTab = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<Tab>("PanilhaNovaTab");
            if (panilhaCompletaTab is null)
                panilhaCompletaTab = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<Tab>("PanilhaCompletaTab");
            if (panilhaMenorTab is null)
                panilhaMenorTab = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<Tab>("PanilhaMenorTab");

            AoNotificar_AlterarVisualizacao();
        }


        void AoNotificar_AlterarVisualizacao() {
            if (panilhaTabView is null)
                return;
            if (!Jogo.EhValido(LivroJogo.INSTANCIA.jogoAtual, false)) {
                panilhaTabView.activeTab = menuInicialTab;
                livroJogoMotor.bookPageCurlMotor.ImporPaginaAtual(0);
                return;
            }
            if (!PaginaExecutora.EhValido(LivroJogo.INSTANCIA.paginaExecutora))
                return;
            paginaPanilha.style.visibility = Uteis.ObterVisibility(true);
            if ((LivroJogo.INSTANCIA.paginaExecutora.idPagina == 1) && (LivroJogo.INSTANCIA.paginaExecutora.idCapitulo == CAMPANHA_CAPITULO.PAGINAS_INICIAIS)) {
                panilhaTabView.activeTab = panilhaNovaTab;
                return;
            }
            if (livroJogoMotor.visualizacao == VISUALIZACAO.PANILHA)
                panilhaTabView.activeTab = panilhaCompletaTab;
            else if (livroJogoMotor.visualizacao == VISUALIZACAO.TUDO)
                panilhaTabView.activeTab = panilhaCompletaTab;
            else if (livroJogoMotor.visualizacao == VISUALIZACAO.CAMPANHA)
                panilhaTabView.activeTab = panilhaMenorTab;
        }
    }
}