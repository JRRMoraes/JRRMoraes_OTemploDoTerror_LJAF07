using Assets.Scripts.Tipos;
using UnityEngine;
using UnityEngine.UIElements;
using static Assets.Scripts.Tipos.Conjuntos;


namespace Assets.Scripts.Componentes {

    public class PaginaCampanha : MonoBehaviour, IPadraoObservador {

        LivroJogoMotor livroJogoMotor;

        VisualElement paginaCampanha;

        VisualElement campanhaTituloVE;

        Label campanhaTitulo;



        void Awake() {
            livroJogoMotor = GetComponent<LivroJogoMotor>();
            LivroJogo.INSTANCIA.observadoresAlvos.Inscrever(this);
        }


        public void AoNotificar(OBSERVADOR_CONDICAO observadorCondicao) {
            if (!LivroJogoMotor.EhValido(livroJogoMotor))
                return;
            if (paginaCampanha is null) {
                paginaCampanha = livroJogoMotor.Raiz_PaginaDireitaCampanha().Query<VisualElement>("PaginaCampanha");
                if (paginaCampanha is null)
                    return;
            }
            if (campanhaTituloVE is null) {
                campanhaTituloVE = livroJogoMotor.Raiz_PaginaDireitaCampanha().Query<VisualElement>("CampanhaTituloVE");
                if (campanhaTituloVE is null)
                    return;
            }
            if (campanhaTitulo is null) {
                campanhaTitulo = livroJogoMotor.Raiz_PaginaDireitaCampanha().Query<Label>("CampanhaTitulo");
                if (campanhaTitulo is null)
                    return;
            }

            if (AoNotificar_ExecutarPaginaPanilha(observadorCondicao))
                return;
        }


        bool AoNotificar_ExecutarPaginaPanilha(OBSERVADOR_CONDICAO observadorCondicao) {
            if (observadorCondicao != OBSERVADOR_CONDICAO.PAGINA_EXECUTORA)
                return false;
            bool _limpa = (!PaginaExecutora.EhValido(LivroJogo.INSTANCIA.paginaExecutora))
                || (LivroJogo.INSTANCIA.paginaExecutora.paginaEstado == PAGINA_EXECUTOR_ESTADO.NULO);
            if (_limpa) {
                paginaCampanha.style.display = Uteis.ObterDisplayStyle(false);
                campanhaTitulo.text = "";
                campanhaTitulo.ClearClassList();
                campanhaTitulo.RemoveFromHierarchy();
                campanhaTituloVE.Add(campanhaTitulo);
                return true;
            }
            if (LivroJogo.INSTANCIA.paginaExecutora.paginaEstado != PAGINA_EXECUTOR_ESTADO.INICIALIZADO)
                return false;
            paginaCampanha.style.display = Uteis.ObterDisplayStyle(true);
            if (!string.IsNullOrWhiteSpace(LivroJogo.INSTANCIA.paginaExecutora.titulo)) {
                campanhaTitulo.text = LivroJogo.INSTANCIA.paginaExecutora.titulo;
                campanhaTitulo.AddToClassList("campanhaTituloTexto");
            }
            else if (LivroJogo.INSTANCIA.paginaExecutora.idPagina >= 1) {
                campanhaTitulo.text = LivroJogo.INSTANCIA.paginaExecutora.idPagina.ToString();
                campanhaTitulo.AddToClassList("campanhaTituloNumero");
            }
            return true;
        }
    }
}