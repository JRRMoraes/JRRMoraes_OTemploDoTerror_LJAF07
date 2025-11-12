using UnityEngine;
using UnityEngine.UIElements;


namespace Assets.Scripts {

    public class LivroJogoMotor : MonoBehaviour {

        public Book book;

        public AutoFlip autoFlip;

        public UIDocument uiDocument;

        VisualElement raiz;

        public VisualElement paginaPanilha;

        public VisualElement paginaCampanha;

        public VisualElement paginaTitulo;

        public VisualElement historia;

        public VisualElement combate;

        public VisualElement destino;

        public VisualElement panilha;

        public VisualElement panilhaNova;


        void Start() {
            raiz = uiDocument.rootVisualElement;
            if (raiz is null)
                return;
            /////
            ///// teste
            if (LivroJogo.INSTANCIA.jogoAtual is null) {
                LivroJogo.INSTANCIA.jogoAtual = LivroJogo.INSTANCIA.jogoSalvo_3.Clonar();
                if (LivroJogo.INSTANCIA.paginaAtual.idPagina == 1)
                    LivroJogo.INSTANCIA.paginaAtual.idPagina = 3;
            }
            /////
            /////
            raiz.dataSource = LivroJogo.INSTANCIA;
            paginaPanilha = raiz.Query<VisualElement>("PaginaPanilha");
            paginaCampanha = raiz.Query<VisualElement>("PaginaCampanha");
            paginaTitulo = raiz.Query<VisualElement>("PaginaTitulo");
            //            if (titulo != null)
            //                titulo.data
            historia = raiz.Query<VisualElement>("Historia");
            combate = raiz.Query<VisualElement>("Combate");
            destino = raiz.Query<VisualElement>("Destino");
            panilha = raiz.Query<VisualElement>("Panilha");
            panilhaNova = raiz.Query<VisualElement>("PanilhaNova");
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
    }
}
