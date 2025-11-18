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

        VisualElement panilhaNova;

        VisualElement panilhaCompleta;

        VisualElement panilhaMenor;



        void Awake() {
            livroJogoMotor = GetComponent<LivroJogoMotor>();
            LivroJogo.INSTANCIA.observadoresAlvos.Inscrever(this);
        }


        public void AoNotificar(OBSERVADOR_CONDICAO observadorCondicao) {
            if (!LivroJogoMotor.EhValido(livroJogoMotor))
                return;
            if (!OBSERVADOR_CONDICAO__VisualizacaoEPaginaExecutora.Contains(observadorCondicao))
                return;
            if (paginaPanilha is null)
                paginaPanilha = livroJogoMotor.raiz.Query<VisualElement>("PaginaPanilha");
            if (panilhaNova is null)
                panilhaNova = livroJogoMotor.raiz.Query<VisualElement>("PanilhaNova");
            if (panilhaCompleta is null)
                panilhaCompleta = livroJogoMotor.raiz.Query<VisualElement>("PanilhaCompleta");
            if (panilhaMenor is null)
                panilhaMenor = livroJogoMotor.raiz.Query<VisualElement>("PanilhaMenor");
            AoNotificar_AlterarVisualizacao();
        }


        void AoNotificar_AlterarVisualizacao() {
            if ((paginaPanilha is null) || (!PaginaExecutora.EhValido(LivroJogo.INSTANCIA.paginaExecutora)))
                return;
            bool _ehPanilhaNova = (LivroJogo.INSTANCIA.paginaExecutora.idPagina == 1)
                && (LivroJogo.INSTANCIA.paginaExecutora.idCapitulo == CAMPANHA_CAPITULO.PAGINAS_INICIAIS);
            if (livroJogoMotor.visualizacao == VISUALIZACAO.PANILHA) {
                panilhaNova.style.visibility = Uteis.ObterVisibility(_ehPanilhaNova);
                panilhaCompleta.style.visibility = Uteis.ObterVisibility((!_ehPanilhaNova) && (true));
                panilhaMenor.style.visibility = Uteis.ObterVisibility((!_ehPanilhaNova) && (false));
                paginaPanilha.style.width = new Length(75, LengthUnit.Percent);
            }
            else if (livroJogoMotor.visualizacao == VISUALIZACAO.TUDO) {
                panilhaNova.style.visibility = Uteis.ObterVisibility(_ehPanilhaNova);
                panilhaCompleta.style.visibility = Uteis.ObterVisibility((!_ehPanilhaNova) && (true));
                panilhaMenor.style.visibility = Uteis.ObterVisibility((!_ehPanilhaNova) && (false));
                paginaPanilha.style.width = new Length(50, LengthUnit.Percent);
            }
            else if (livroJogoMotor.visualizacao == VISUALIZACAO.CAMPANHA) {
                panilhaNova.style.visibility = Uteis.ObterVisibility(_ehPanilhaNova);
                panilhaCompleta.style.visibility = Uteis.ObterVisibility((!_ehPanilhaNova) && (false));
                panilhaMenor.style.visibility = Uteis.ObterVisibility((!_ehPanilhaNova) && (true));
                paginaPanilha.style.width = new Length(25, LengthUnit.Percent);
            }
        }
    }
}