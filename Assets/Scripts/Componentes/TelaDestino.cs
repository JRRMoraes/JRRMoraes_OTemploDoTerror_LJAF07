using Assets.Scripts.Tipos;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static Assets.Scripts.Tipos.Conjuntos;
using Button = UnityEngine.UIElements.Button;
using Image = UnityEngine.UIElements.Image;


namespace Assets.Scripts.Componentes {

    [RequireComponent(typeof(LivroJogoMotor))]
    public class TelaDestino : MonoBehaviour, IPadraoObservador {

        LivroJogoMotor livroJogoMotor;

        VisualElement paginaPanilha;

        VisualElement paginaCampanha;

        //[SerializeField] private VisualTreeAsset uxmlTelaDestinoSelecaoButton;
        //private VisualElement raizTelaDestinoSelecaoButton;

        VisualElement telaDestino;

        VisualElement destinoComandosGroupBox;

        VisualElement destinoSelecaoGroupBox;

        Button salvarJogoAtualButton;

        Button curarJogadorButton;

        RoladorDeDados roladorDeDados;


        void Awake() {
            livroJogoMotor = GetComponent<LivroJogoMotor>();
            LivroJogo.INSTANCIA.observadoresAlvos.Inscrever(this);
        }


        void OnDestroy() {
            LivroJogo.INSTANCIA.observadoresAlvos.Desinscrever(this);
        }


        public PaginaExecutora PaginaExecutoraAtual() {
            return LivroJogo.INSTANCIA.paginaExecutora;
        }


        public Jogo JogoAtual() {
            return LivroJogo.INSTANCIA.jogoAtual;
        }


        public void AoNotificar(OBSERVADOR_CONDICAO observadorCondicao) {
            if (!LivroJogoMotor.EhValido(livroJogoMotor))
                return;
            if (paginaPanilha is null)
                paginaPanilha = livroJogoMotor.Raiz_PaginaEsquerdaPanilha().Query<VisualElement>("PaginaPanilha");
            if (paginaCampanha is null)
                paginaCampanha = livroJogoMotor.Raiz_PaginaDireitaCampanha().Query<VisualElement>("PaginaCampanha");

            if (telaDestino is null)
                telaDestino = livroJogoMotor.Raiz_PaginaDireitaCampanha().Query<VisualElement>("TelaDestino");



            if (destinoComandosGroupBox is null)
                destinoComandosGroupBox = livroJogoMotor.Raiz_PaginaDireitaCampanha().Query<VisualElement>("DestinoComandosGroupBox");
            if (salvarJogoAtualButton is null) {
                salvarJogoAtualButton = livroJogoMotor.Raiz_PaginaDireitaCampanha().Query<Button>("SalvarJogoAtualButton");
                if (salvarJogoAtualButton != null)
                    salvarJogoAtualButton.RegisterCallback<ClickEvent>(AoSalvarJogo);
            }
            if (curarJogadorButton is null) {
                curarJogadorButton = livroJogoMotor.Raiz_PaginaDireitaCampanha().Query<Button>("CurarJogadorButton");
                if (curarJogadorButton != null)
                    curarJogadorButton.RegisterCallback<ClickEvent>(AoCurarJogador);
            }
            if (destinoSelecaoGroupBox is null)
                destinoSelecaoGroupBox = livroJogoMotor.Raiz_PaginaDireitaCampanha().Query<VisualElement>("DestinoSelecaoGroupBox");

            if ((destinoComandosGroupBox is null) || (destinoSelecaoGroupBox is null))
                return;
            if (!PaginaExecutora.EhValido(PaginaExecutoraAtual())) {
                destinoComandosGroupBox.style.visibility = Uteis.ObterVisibility(false);
                while (destinoSelecaoGroupBox.childCount >= 1)
                    destinoSelecaoGroupBox.Remove(destinoSelecaoGroupBox.Children().First());
                return;
            }
            if (PaginaExecutoraAtual().paginaEstado != PAGINA_EXECUTOR_ESTADO.DESTINOS)
                return;
            if (!Jogo.EhValido(LivroJogo.INSTANCIA.jogoAtual, false))
                return;

            if (AoNotificar_ProcessarSalvamento(observadorCondicao))
                return;
            if (AoNotificar_ProcessarCurando(observadorCondicao))
                return;
            if (AoNotificar_ProcessarTesteAtributo(observadorCondicao))
                return;
            if (AoNotificar_ProcessarTelaDestino(observadorCondicao))
                return;
        }


        void ProcessarDestinoDesativaBotoes(bool destinoDesativaBotoes) {
            PaginaExecutoraAtual().destinoDesativaBotoes = destinoDesativaBotoes;
            if (PaginaExecutoraAtual().destinoDesativaBotoes) {
                salvarJogoAtualButton.SetEnabled(false);
                curarJogadorButton.SetEnabled(false);
                foreach (DestinoExecucao _destinoI in PaginaExecutoraAtual().destinoItens) {
                    if (_destinoI.selecaoButton != null)
                        _destinoI.selecaoButton.SetEnabled(false);
                }
            }
            else {
                salvarJogoAtualButton.SetEnabled((PaginaExecutoraAtual().destinoProcessoSalvando == PROCESSO.ZERO));
                curarJogadorButton.SetEnabled((PaginaExecutoraAtual().destinoProcessoCurando == PROCESSO.ZERO)
                    && (LivroJogo.INSTANCIA.jogoAtual.panilha != null)
                    && (LivroJogo.INSTANCIA.jogoAtual.panilha.provisao >= 1));
                foreach (DestinoExecucao _destinoI in PaginaExecutoraAtual().destinoItens) {
                    if (_destinoI.selecaoButton != null)
                        _destinoI.selecaoButton.SetEnabled(JogoAtualValidacoes.ValidarAprovacoesDestino(_destinoI.aprovacoes));
                }
            }
        }


        bool AoNotificar_ProcessarSalvamento(OBSERVADOR_CONDICAO observadorCondicao) {
            if (!OBSERVADOR_CONDICAO__JogoAtualEPaginaExecutora.Contains(observadorCondicao))
                return false;
            if (PaginaExecutoraAtual().destinoProcesso != PROCESSO.PROCESSANDO) {
                salvarJogoAtualButton.SetEnabled(true);
                salvarJogoAtualButton.style.display = Uteis.ObterDisplayStyle(false);
                return false;
            }
            switch (PaginaExecutoraAtual().destinoProcessoSalvando) {
                case PROCESSO.ZERO:
                    if (!LivroJogo.INSTANCIA.ehJogoCarregado)
                        salvarJogoAtualButton.style.display = Uteis.ObterDisplayStyle(true);
                    else
                        PaginaExecutoraAtual().destinoProcessoSalvando = PROCESSO.DESTRUIDO;
                    ProcessarDestinoDesativaBotoes(false);
                    return false;
                case PROCESSO.INICIANDO:
                    PaginaExecutoraAtual().destinoProcessoSalvando = PROCESSO.PROCESSANDO;
                    LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                    StartCoroutine(AguardarProcessoSalvamentoProcessandoParaConcluido());
                    return true;
                case PROCESSO.CONCLUIDO:
                    salvarJogoAtualButton.text = "JOGO SALVO !!!";
                    PaginaExecutoraAtual().destinoProcessoSalvando = PROCESSO.DESTRUIDO;
                    LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                    ProcessarDestinoDesativaBotoes(false);
                    return true;
            }
            return false;
        }


        IEnumerator AguardarProcessoSalvamentoProcessandoParaConcluido() {
            yield return null;
            LivroJogo.INSTANCIA.SalvarJogoAtualNoJogoSalvo();
            yield return new WaitForSeconds(Constantes.TEMPO_ANIMACAO_NORMAL);
            PaginaExecutoraAtual().destinoProcessoSalvando = PROCESSO.CONCLUIDO;
            LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
        }


        bool AoNotificar_ProcessarCurando(OBSERVADOR_CONDICAO observadorCondicao) {
            if (!OBSERVADOR_CONDICAO__JogoAtualEPaginaExecutora.Contains(observadorCondicao))
                return false;
            if (PaginaExecutoraAtual().destinoProcesso != PROCESSO.PROCESSANDO) {
                curarJogadorButton.SetEnabled(true);
                curarJogadorButton.style.display = Uteis.ObterDisplayStyle(false);
                return false;
            }
            switch (PaginaExecutoraAtual().destinoProcessoCurando) {
                case PROCESSO.ZERO:
                    if (LivroJogo.INSTANCIA.jogoAtual.panilha is null) {
                        PaginaExecutoraAtual().destinoProcessoCurando = PROCESSO.DESTRUIDO;
                    }
                    else if (LivroJogo.INSTANCIA.jogoAtual.panilha.provisao >= 1) {
                        curarJogadorButton.text = $"CURAR-SE ?  ( {LivroJogo.INSTANCIA.jogoAtual.panilha.provisao.ToString()} provisões )";
                        curarJogadorButton.style.display = Uteis.ObterDisplayStyle(true);
                    }
                    else {
                        curarJogadorButton.text = $"SEM CURA  ( 0 provisões )";
                        curarJogadorButton.style.display = Uteis.ObterDisplayStyle(true);
                        PaginaExecutoraAtual().destinoProcessoCurando = PROCESSO.DESTRUIDO;
                    }
                    ProcessarDestinoDesativaBotoes(false);
                    return false;
                case PROCESSO.INICIANDO:
                    PaginaExecutoraAtual().destinoProcessoCurando = PROCESSO.PROCESSANDO;
                    LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                    StartCoroutine(AguardarProcessoCurandoProcessandoParaConcluido());
                    return true;
                case PROCESSO.CONCLUIDO:
                    PaginaExecutoraAtual().destinoProcessoCurando = PROCESSO.ZERO;
                    LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                    ProcessarDestinoDesativaBotoes(false);
                    return true;
            }
            return false;
        }


        IEnumerator AguardarProcessoCurandoProcessandoParaConcluido() {
            yield return null;
            ////     AplicarCuraEnergiaECustoProvisao();
            yield return new WaitForSeconds(Constantes.TEMPO_ANIMACAO_NORMAL);
            PaginaExecutoraAtual().destinoProcessoCurando = PROCESSO.CONCLUIDO;
            LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
        }


        bool AoNotificar_ProcessarTesteAtributo(OBSERVADOR_CONDICAO observadorCondicao) {
            if (!OBSERVADOR_CONDICAO__JogoAtualEPaginaExecutora.Contains(observadorCondicao))
                return false;
            if (PaginaExecutoraAtual().destinoProcesso != PROCESSO.PROCESSANDO)
                return false;
            switch (PaginaExecutoraAtual().destinoProcessoRolagem) {
                case PROCESSO.INICIANDO:
                    PaginaExecutoraAtual().destinoRolagemTotal = 0;
                    roladorDeDados.Rolar();
                    PaginaExecutoraAtual().destinoProcessoRolagem = PROCESSO.PROCESSANDO;
                    LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                    ///                        StartCoroutine(AguardarProcessoSalvamentoProcessandoParaConcluido());
                    return true;
                case PROCESSO.CONCLUIDO:
                    PaginaExecutoraAtual().destinoRolagemTotal += PaginaExecutoraAtual().destinoRolagemDestino.testeSomarDados;
                    bool _teveSorte = false;
                    if (PaginaExecutoraAtual().destinoRolagemDestino.testeAtributo == ATRIBUTO_DESTINO_TESTE.HABILIDADE) {
                        _teveSorte = (PaginaExecutoraAtual().destinoRolagemTotal <= LivroJogo.INSTANCIA.jogoAtual.panilha.habilidade);
                    }
                    else if (PaginaExecutoraAtual().destinoRolagemDestino.testeAtributo == ATRIBUTO_DESTINO_TESTE.SORTE) {
                        _teveSorte = (PaginaExecutoraAtual().destinoRolagemTotal <= LivroJogo.INSTANCIA.jogoAtual.panilha.sorte);
                        /////   AplicarPenalidadeDeTestarSorte();
                    }
                    int _idPagina = (_teveSorte) ? PaginaExecutoraAtual().destinoRolagemDestino.idPagina : PaginaExecutoraAtual().destinoRolagemDestino.idPaginaAzar;
                    PaginaExecutoraAtual().paginaIdPaginaDestino = _idPagina;
                    PaginaExecutoraAtual().paginaIdCapituloDestino = PaginaExecutoraAtual().destinoRolagemDestino.idCapitulo;
                    PaginaExecutoraAtual().destinoProcessoRolagem = PROCESSO.DESTRUIDO;
                    LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                    return true;
            }
            return false;
        }


        bool AoNotificar_ProcessarTelaDestino(OBSERVADOR_CONDICAO observadorCondicao) {
            if (!OBSERVADOR_CONDICAO__JogoAtualEPaginaExecutora.Contains(observadorCondicao))
                return false;
            switch (PaginaExecutoraAtual().destinoProcesso) {
                case PROCESSO.ZERO:
                    if (LivroJogo.INSTANCIA.EhFimDeJogo()) {
                        MontarElementosDeFimDeJogo();
                        PaginaExecutoraAtual().destinoProcesso = PROCESSO.CONCLUIDO;
                        LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                        return true;
                    }
                    if (PaginaExecutoraAtual().destinos != null)
                        PaginaExecutoraAtual().destinoProcesso = PROCESSO.INICIANDO;
                    else
                        PaginaExecutoraAtual().destinoProcesso = PROCESSO.CONCLUIDO;
                    PaginaExecutoraAtual().destinoDesativaBotoes = false;
                    PaginaExecutoraAtual().destinoProcessoSalvando = PROCESSO.ZERO;
                    PaginaExecutoraAtual().destinoProcessoCurando = PROCESSO.ZERO;
                    LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                    return true;
                case PROCESSO.INICIANDO:
                    destinoComandosGroupBox.style.visibility = Uteis.ObterVisibility(!LivroJogo.INSTANCIA.ehJogoCarregado);
                    MontarElementosDeDestinosSelecoes();
                    PaginaExecutoraAtual().destinoProcesso = PROCESSO.PROCESSANDO;
                    LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                    return true;
                case PROCESSO.PROCESSANDO:
                    if ((PaginaExecutoraAtual().paginaIdPaginaDestino == PaginaUtils.PAGINA_ZERADA().idPagina) && (PaginaExecutoraAtual().paginaIdCapituloDestino == PaginaUtils.PAGINA_ZERADA().idCapitulo))
                        return false;
                    if (paginaPanilha != null)
                        paginaPanilha.style.visibility = Uteis.ObterVisibility(false);
                    if (paginaCampanha != null)
                        paginaCampanha.style.visibility = Uteis.ObterVisibility(false);
                    StartCoroutine(AguardarProcessoDestinoProcessandoParaConcluido());////?????????
                    return false;
                case PROCESSO.CONCLUIDO:
                    if ((PaginaExecutoraAtual().paginaIdPaginaDestino == PaginaUtils.PAGINA_ZERADA().idPagina) && (PaginaExecutoraAtual().paginaIdCapituloDestino == PaginaUtils.PAGINA_ZERADA().idCapitulo))
                        return false;
                    PaginaExecutoraAtual().destinoProcesso = PROCESSO.DESTRUIDO;
                    PaginaExecutoraAtual().paginaEstado = PAGINA_EXECUTOR_ESTADO.DESTRUIDO;
                    LivroJogo.INSTANCIA.ehJogoCarregado = false;
                    LivroJogo.INSTANCIA.ImporCampanhaDestinoNoJogoAtual(PaginaExecutoraAtual().paginaIdPaginaDestino, PaginaExecutoraAtual().paginaIdCapituloDestino);
                    LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                    return true;
            }
            return false;
        }


        void AoReiniciarJogo(ClickEvent evento) {
            ProcessarDestinoDesativaBotoes(true);
            LivroJogo.INSTANCIA.ResetarJogo();
            SceneManager.LoadScene("LivroJogo");
        }


        void MontarElementosDeFimDeJogo() {
            destinoComandosGroupBox.style.visibility = Uteis.ObterVisibility(false);
            Label _fimDeJogoLabel = new Label();
            _fimDeJogoLabel.text = "VOCÊ MORREU  -  FIM DE JOGO";
            _fimDeJogoLabel.AddToClassList("destinoTexto");
            _fimDeJogoLabel.AddToClassList("destinoMorte");
            destinoSelecaoGroupBox.Add(_fimDeJogoLabel);
            Button _reiniciarJogoButton = new Button();
            _reiniciarJogoButton.text = "Voltar ao Menu Inicial";
            _reiniciarJogoButton.AddToClassList("destinoSelecao");
            _reiniciarJogoButton.AddToClassList("destinoMorte");
            _reiniciarJogoButton.RegisterCallback<ClickEvent>(AoReiniciarJogo);
            destinoSelecaoGroupBox.Add(_reiniciarJogoButton);
        }


        void MontarElementosDeDestinosSelecoes() {
            foreach (DestinoExecucao _destinoI in PaginaExecutoraAtual().destinoItens) {
                /////raizTelaDestinoSelecaoButton = uxmlTelaDestinoSelecaoButton.Instantiate();
                /////destinoSelecaoGroupBox.rootVisualElement.Add(raizTelaDestinoSelecaoButton);
                Button _destinoSelecaoButton = new Button();
                _destinoI.selecaoButton = _destinoSelecaoButton;
                _destinoSelecaoButton.AddToClassList("destinoSelecao");
                _destinoSelecaoButton.RegisterCallback<ClickEvent>((evento) => AoSelecionarDestino(evento, _destinoI));
                //                desativado ={ AoObterDesativaBotao(destinoI)}
                MontarDestinoSelecaoButtonTextos(_destinoI, _destinoSelecaoButton);
                MontarDestinoSelecaoButtonTesteAtributo(_destinoI, _destinoSelecaoButton);
                if (!string.IsNullOrWhiteSpace(_destinoI.imagemArquivo))
                    StartCoroutine(MontarDestinoSelecaoButtonImagem(_destinoI, _destinoSelecaoButton));
                destinoSelecaoGroupBox.Add(_destinoSelecaoButton);
            }
            ProcessarDestinoDesativaBotoes(false);
        }


        void AoSelecionarDestino(ClickEvent evento, DestinoExecucao destinoExecucao) {
            ProcessarDestinoDesativaBotoes(true);
            if (destinoExecucao.testeAtributo == ATRIBUTO_DESTINO_TESTE.NULO) {
                PaginaExecutoraAtual().paginaIdPaginaDestino = destinoExecucao.idPagina;
                PaginaExecutoraAtual().paginaIdCapituloDestino = destinoExecucao.idCapitulo;
                LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
            }
            else {
                PaginaExecutoraAtual().destinoRolagemDestino = destinoExecucao;
                PaginaExecutoraAtual().destinoProcessoRolagem = PROCESSO.INICIANDO;
                LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
            }
        }


        void AoSalvarJogo(ClickEvent evento) {
            ProcessarDestinoDesativaBotoes(true);
            PaginaExecutoraAtual().destinoProcessoSalvando = PROCESSO.INICIANDO;
            LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
        }


        void AoCurarJogador(ClickEvent evento) {
            ProcessarDestinoDesativaBotoes(true);
            PaginaExecutoraAtual().destinoProcessoCurando = PROCESSO.INICIANDO;
            LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
        }


        void MontarDestinoSelecaoButtonTextos(DestinoExecucao destinoExecucao, Button destinoSelecaoButton) {
            if ((destinoExecucao.textosDestino != null) && (destinoExecucao.textosDestino.Length >= 1)) {
                foreach (string _textoDestinoI in destinoExecucao.textosDestino) {
                    Label _textoDestinoLabel = new Label();
                    _textoDestinoLabel.text = _textoDestinoI;
                    if (string.IsNullOrWhiteSpace(_textoDestinoLabel.text))
                        _textoDestinoLabel.text = $"???  {destinoExecucao.idPagina}  ???";
                    _textoDestinoLabel.AddToClassList("historiaTexto");
                    destinoSelecaoButton.Add(_textoDestinoLabel);
                }
            }
            else {
                Label _textoDestinoLabel = new Label();
                _textoDestinoLabel.text = destinoExecucao.textoDestino;
                if (string.IsNullOrWhiteSpace(_textoDestinoLabel.text))
                    _textoDestinoLabel.text = $"???  {destinoExecucao.idPagina}  ???";
                _textoDestinoLabel.AddToClassList("historiaTexto");
                destinoSelecaoButton.Add(_textoDestinoLabel);
            }
        }


        void MontarDestinoSelecaoButtonTesteAtributo(DestinoExecucao destinoExecucao, Button destinoSelecaoButton) {
            if (destinoExecucao.testeAtributo == ATRIBUTO_DESTINO_TESTE.NULO)
                return;
            roladorDeDados = new RoladorDeDados();
            roladorDeDados.roladorDeDadosMotor = GetComponent<RoladorDeDadosMotor>();
            roladorDeDados.raizVisualElement = destinoSelecaoButton;
            if (destinoExecucao.testeAtributo == ATRIBUTO_DESTINO_TESTE.HABILIDADE) {
                roladorDeDados.dadosRolados.Add(new DadoRolado(DADO.D6_VERDE, "dadoTesteAtributo0"));
                roladorDeDados.dadosRolados.Add(new DadoRolado(DADO.D6_VERDE, "dadoTesteAtributo1"));
            }
            else {
                roladorDeDados.dadosRolados.Add(new DadoRolado(DADO.D6_DOURADO, "dadoTesteAtributo0"));
                roladorDeDados.dadosRolados.Add(new DadoRolado(DADO.D6_DOURADO, "dadoTesteAtributo1"));
            }
            VisualElement _destinoSelecaoAtributo = new VisualElement();
            _destinoSelecaoAtributo.AddToClassList("destinoSelecaoAtributo");
            destinoSelecaoButton.Add(_destinoSelecaoAtributo);
            VisualElement _dadoImagem0 = new VisualElement();
            _dadoImagem0.name = "dadoTesteAtributo0";
            _dadoImagem0.AddToClassList("dadoVE");
            _destinoSelecaoAtributo.Add(_dadoImagem0);
            VisualElement _dadoImagem1 = new VisualElement();
            _dadoImagem1.name = "dadoTesteAtributo1";
            _dadoImagem1.AddToClassList("dadoVE");
            _destinoSelecaoAtributo.Add(_dadoImagem1);
            roladorDeDados.OnResultadoFinal = TesteAtributoResultadoFinal;
            roladorDeDados.Iniciar();
        }


        void TesteAtributoResultadoFinal(int[] resultados) {
            if (resultados != null) {
                foreach (int _itemI in resultados)
                    PaginaExecutoraAtual().destinoRolagemTotal += _itemI;
            }
            PaginaExecutoraAtual().destinoProcessoRolagem = PROCESSO.CONCLUIDO;
            LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
        }


        IEnumerator MontarDestinoSelecaoButtonImagem(DestinoExecucao destinoExecucao, Button destinoSelecaoButton) {
            if (string.IsNullOrWhiteSpace(destinoExecucao.imagemArquivo))
                yield break;
            if (!File.Exists(destinoExecucao.imagemArquivo))
                yield break;
            Task<Texture2D> _textura = Uteis.CarregarImagemAsync(destinoExecucao.imagemArquivo);
            while (!_textura.IsCompleted)
                yield return null;
            if (_textura.Result is null)
                yield break;
            Image _imagem = new Image();
            _imagem.image = _textura.Result;
            _imagem.scaleMode = ScaleMode.ScaleToFit;
            //_imagem.style.width = 200;
            //_imagem.style.height = 200;
            _imagem.AddToClassList("historiaImagem");
            destinoSelecaoButton.Add(_imagem);
        }


        IEnumerator AguardarProcessoDestinoProcessandoParaConcluido() {
            yield return null;
            livroJogoMotor.PassarPaginasNoBookAutoFlip(JogoAtual().campanhaIdPagina, PaginaExecutoraAtual().paginaIdPaginaDestino);
            yield return new WaitForSeconds(Constantes.TEMPO_ANIMACAO_NORMAL);
            PaginaExecutoraAtual().destinoProcessoCurando = PROCESSO.CONCLUIDO;
            LivroJogo.INSTANCIA.observadoresAlvos.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
        }
    }
}