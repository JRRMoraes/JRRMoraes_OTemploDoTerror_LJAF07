using Assets.Scripts.Tipos;
using UnityEngine;
using UnityEngine.UIElements;
using static Assets.Scripts.Tipos.Conjuntos;


namespace Assets.Scripts.Componentes {

    public class PaginaCampanha : MonoBehaviour, IPadraoObservador {

        LivroJogoMotor livroJogoMotor;

        VisualElement paginaCampanha;

        Label paginaTitulo;



        void Awake() {
            livroJogoMotor = GetComponent<LivroJogoMotor>();
            if (LivroJogoMotor.EhValido(livroJogoMotor, false))
                livroJogoMotor.observadorAlvo_Visualizacao.Inscrever(this);
        }


        public void AoNotificar(OBSERVADOR_CONDICAO observadorCondicao) {
            if (!LivroJogoMotor.EhValido(livroJogoMotor))
                return;
            if (paginaCampanha is null)
                paginaCampanha = livroJogoMotor.raiz.Query<VisualElement>("PaginaCampanha");
            if (paginaTitulo is null)
                paginaTitulo = livroJogoMotor.raiz.Query<Label>("PaginaTitulo");

            if (AoNotificar_AlterarVisualizacao(observadorCondicao))
                return;
            if (AoNotificar_ExecutarPaginaPanilha(observadorCondicao))
                return;
        }


        bool AoNotificar_AlterarVisualizacao(OBSERVADOR_CONDICAO observadorCondicao) {
            if (observadorCondicao != OBSERVADOR_CONDICAO.VISUALIZACAO)
                return false;
            if (paginaCampanha is null)
                return false;
            if (livroJogoMotor.visualizacao == VISUALIZACAO.PANILHA) {
                paginaCampanha.style.width = new Length(25, LengthUnit.Percent);
            }
            else if (livroJogoMotor.visualizacao == VISUALIZACAO.TUDO) {
                paginaCampanha.style.width = new Length(50, LengthUnit.Percent);
            }
            else if (livroJogoMotor.visualizacao == VISUALIZACAO.CAMPANHA) {
                paginaCampanha.style.width = new Length(75, LengthUnit.Percent);
            }
            return true;
        }


        bool AoNotificar_ExecutarPaginaPanilha(OBSERVADOR_CONDICAO observadorCondicao) {
            if (observadorCondicao != OBSERVADOR_CONDICAO.PAGINA_EXECUTORA)
                return false;
            if ((paginaCampanha is null) || (paginaTitulo is null))
                return false;
            if (!PaginaExecutora.EhValido(LivroJogo.INSTANCIA.paginaExecutora)) {
                paginaTitulo.text = "";
                return true;
            }
            if (LivroJogo.INSTANCIA.paginaExecutora.paginaEstado != PAGINA_EXECUTOR_ESTADO.INICIALIZADO)
                return false;
            if (!string.IsNullOrWhiteSpace(LivroJogo.INSTANCIA.paginaExecutora.titulo))
                paginaTitulo.text = LivroJogo.INSTANCIA.paginaExecutora.titulo;
            else if (LivroJogo.INSTANCIA.paginaExecutora.idPagina >= 1)
                paginaTitulo.text = LivroJogo.INSTANCIA.paginaExecutora.idPagina.ToString();
            return true;
        }
    }
}