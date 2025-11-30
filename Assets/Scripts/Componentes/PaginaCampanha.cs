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
            if (paginaCampanha is null)
                paginaCampanha = livroJogoMotor.Raiz_PaginaDireitaCampanha().Query<VisualElement>("PaginaCampanha");
            if (campanhaTituloVE is null)
                campanhaTituloVE = livroJogoMotor.Raiz_PaginaDireitaCampanha().Query<VisualElement>("CampanhaTituloVE");
            if (campanhaTitulo is null)
                campanhaTitulo = livroJogoMotor.Raiz_PaginaDireitaCampanha().Query<Label>("CampanhaTitulo");

            if (AoNotificar_ExecutarPaginaPanilha(observadorCondicao))
                return;
        }


        bool AoNotificar_ExecutarPaginaPanilha(OBSERVADOR_CONDICAO observadorCondicao) {
            if (observadorCondicao != OBSERVADOR_CONDICAO.PAGINA_EXECUTORA)
                return false;
            if ((campanhaTituloVE is null) || (campanhaTitulo is null))
                return false;
            if (!PaginaExecutora.EhValido(LivroJogo.INSTANCIA.paginaExecutora)) {
                campanhaTitulo.text = "";
                campanhaTitulo.ClearClassList();
                campanhaTitulo.RemoveFromHierarchy();
                campanhaTituloVE.Add(campanhaTitulo);
                return true;
            }
            if (LivroJogo.INSTANCIA.paginaExecutora.paginaEstado != PAGINA_EXECUTOR_ESTADO.INICIALIZADO)
                return false;
            paginaCampanha.style.visibility = Uteis.ObterVisibility(true);
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