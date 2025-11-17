using Assets.Scripts;
using Assets.Scripts.Tipos;
using System.Collections;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using static Assets.Scripts.Tipos.Conjuntos;


public class TelaHistoria : MonoBehaviour, IPadraoObservador {

    LivroJogoMotor livroJogoMotor;

    VisualElement historiaGroupBox;

    VisualElement comandosGroupBox;

    Button pularHistoriaButton;


    void Awake() {
        livroJogoMotor = GetComponent<LivroJogoMotor>();
        LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Inscrever(this);
    }


    void OnDestroy() {
        if (LivroJogo.INSTANCIA != null)
            LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Desinscrever(this);
    }


    public PaginaExecutora PaginaExecutoraAtual() {
        return LivroJogo.INSTANCIA.paginaExecutora;
    }


    public void AoNotificar(OBSERVADOR_CONDICAO observadorCondicao) {
        if (!LivroJogoMotor.EhValido(livroJogoMotor))
            return;
        if (historiaGroupBox is null)
            historiaGroupBox = livroJogoMotor.raiz.Query<VisualElement>("HistoriaGroupBox");
        if (comandosGroupBox is null)
            comandosGroupBox = livroJogoMotor.raiz.Query<VisualElement>("ComandosGroupBox");
        if (pularHistoriaButton is null) {
            pularHistoriaButton = livroJogoMotor.raiz.Query<Button>("PularHistoriaButton");
            if (pularHistoriaButton != null)
                pularHistoriaButton.RegisterCallback<ClickEvent>(PularHistoria);
        }

        AoNotificar_ProcessarTelaHistoria(observadorCondicao);
    }


    bool AoNotificar_ProcessarTelaHistoria(OBSERVADOR_CONDICAO observadorCondicao) {
        if (!OBSERVADOR_CONDICAO__JogoAtualEPaginaExecutora.Contains(observadorCondicao))
            return false;
        if ((historiaGroupBox is null) || (comandosGroupBox is null))
            return false;
        if (!PaginaExecutora.EhValido(PaginaExecutoraAtual())) {
            comandosGroupBox.style.visibility = Uteis.ObterVisibility(false);
            while (historiaGroupBox.childCount >= 1)
                historiaGroupBox.Remove(historiaGroupBox.Children().First());
            return true;
        }
        if (PaginaExecutoraAtual().paginaEstado != PAGINA_EXECUTOR_ESTADO.HISTORIAS)
            return false;
        if (!Jogo.EhValido(LivroJogo.INSTANCIA.jogoAtual))
            return false;

        switch (PaginaExecutoraAtual().historiaProcesso) {
            case PROCESSO.ZERO:
                if (PaginaExecutoraAtual().historias != null)
                    PaginaExecutoraAtual().historiaProcesso = PROCESSO.INICIANDO;
                else
                    PaginaExecutoraAtual().historiaProcesso = PROCESSO.CONCLUIDO;
                LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                return true;
            case PROCESSO.INICIANDO:
                if (!LivroJogo.INSTANCIA.ehJogoCarregado) {
                    livroJogoMotor.historiaVelocidadeDoTexto = livroJogoMotor.historiaVelocidadeNormalDoTexto;
                    comandosGroupBox.style.visibility = Uteis.ObterVisibility(true);
                }
                else {
                    livroJogoMotor.historiaVelocidadeDoTexto = Constantes.HISTORIA_VELOCIDADE_TEXTO_RAPIDO;
                    comandosGroupBox.style.visibility = Uteis.ObterVisibility(false);
                }
                PaginaExecutoraAtual().historiaIndice = 0;
                PaginaExecutoraAtual().ImporHistoriaTextosExeProcessoTexto(PROCESSO.ZERO);
                PaginaExecutoraAtual().ImporHistoriaEfeitosExeProcessoEfeito(PROCESSO.ZERO);
                PaginaExecutoraAtual().ImporHistoriaImagensExeProcessoImagem(PROCESSO.ZERO);
                PaginaExecutoraAtual().historiaProcessoIndice = PROCESSO_HISTORIA.ZERO;
                PaginaExecutoraAtual().historiaProcesso = PROCESSO.PROCESSANDO;
                LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                return true;

            case PROCESSO.PROCESSANDO:
                switch (PaginaExecutoraAtual().historiaProcessoIndice) {
                    case PROCESSO_HISTORIA.ZERO:
                        PaginaExecutoraAtual().ImporHistoriaTextosExeProcessoTexto(PROCESSO.ZERO);
                        PaginaExecutoraAtual().ImporHistoriaEfeitosExeProcessoEfeito(PROCESSO.ZERO);
                        PaginaExecutoraAtual().ImporHistoriaImagensExeProcessoImagem(PROCESSO.ZERO);
                        PaginaExecutoraAtual().historiaProcessoIndice = PROCESSO_HISTORIA.INICIANDO;
                        LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                        return true;
                    case PROCESSO_HISTORIA.INICIANDO:
                        PaginaExecutoraAtual().historiaProcessoIndice = PROCESSO_HISTORIA.PROCESSANDO_TEXTOS;
                        LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                        return true;
                    case PROCESSO_HISTORIA.PROCESSANDO_TEXTOS:
                        return AoNotificar_ProcessarTelaHistoria_ProcessandoTextos();
                    case PROCESSO_HISTORIA.PROCESSANDO_EFEITOS:
                        return AoNotificar_ProcessarTelaHistoria_ProcessandoEfeitos();
                    case PROCESSO_HISTORIA.PROCESSANDO_IMAGENS:
                        return AoNotificar_ProcessarTelaHistoria_ProcessandoImagens();
                    case PROCESSO_HISTORIA.CONCLUIDO:
                        PaginaExecutoraAtual().historiaIndice++;
                        if (PaginaExecutoraAtual().ObterHistoriaTextosAtuais() is null) {
                            PaginaExecutoraAtual().historiaProcessoIndice = PROCESSO_HISTORIA.DESTRUIDO;
                            PaginaExecutoraAtual().historiaProcesso = PROCESSO.CONCLUIDO;
                        }
                        else {
                            PaginaExecutoraAtual().historiaProcessoIndice = PROCESSO_HISTORIA.ZERO;
                        }
                        LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                        return true;
                }
                return false;

            case PROCESSO.CONCLUIDO:
                comandosGroupBox.style.visibility = Uteis.ObterVisibility(false);
                PaginaExecutoraAtual().historiaProcesso = PROCESSO.DESTRUIDO;
                PaginaExecutoraAtual().paginaEstado = PAGINA_EXECUTOR_ESTADO.COMBATE;
                LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                return true;
        }
        return false;
    }


    bool AoNotificar_ProcessarTelaHistoria_ProcessandoTextos() {
        if ((PaginaExecutoraAtual().ObterHistoriaTextosAtuais() is null) || (PaginaExecutoraAtual().ObterHistoriaTextosAtuais().textosHistoria is null) || (PaginaExecutoraAtual().ObterHistoriaTextosAtuais().textosHistoria.Length <= 0)) {
            PaginaExecutoraAtual().historiaProcessoIndice = PROCESSO_HISTORIA.PROCESSANDO_EFEITOS;
            LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
            return true;
        }
        switch (PaginaExecutoraAtual().ObterHistoriaTextosAtuais().exeProcessoTexto) {
            case PROCESSO.ZERO:
                PaginaExecutoraAtual().ImporHistoriaTextosExeProcessoTexto(PROCESSO.INICIANDO);
                LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                return true;
            case PROCESSO.INICIANDO:
                StartCoroutine(MontarLabelsHistoriaTextos());
                PaginaExecutoraAtual().ImporHistoriaTextosExeProcessoTexto(PROCESSO.PROCESSANDO);
                LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                return true;
            case PROCESSO.CONCLUIDO:
                PaginaExecutoraAtual().ImporHistoriaTextosExeProcessoTexto(PROCESSO.DESTRUIDO);
                PaginaExecutoraAtual().historiaProcessoIndice = PROCESSO_HISTORIA.PROCESSANDO_EFEITOS;
                LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                return true;
        }
        return false;
    }


    bool AoNotificar_ProcessarTelaHistoria_ProcessandoEfeitos() {
        if ((PaginaExecutoraAtual().ObterHistoriaEfeitosAtuais() is null) || (PaginaExecutoraAtual().ObterHistoriaEfeitosAtuais().efeitos is null) || (PaginaExecutoraAtual().ObterHistoriaEfeitosAtuais().efeitos.Length <= 0)) {
            PaginaExecutoraAtual().historiaProcessoIndice = PROCESSO_HISTORIA.PROCESSANDO_IMAGENS;
            LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
            return true;
        }
        switch (PaginaExecutoraAtual().ObterHistoriaEfeitosAtuais().exeProcessoEfeito) {
            case PROCESSO.ZERO:
                PaginaExecutoraAtual().ImporHistoriaEfeitosExeProcessoEfeito(PROCESSO.INICIANDO);
                LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                return true;
            case PROCESSO.INICIANDO:
                if (!LivroJogo.INSTANCIA.ehJogoCarregado) {
                    StartCoroutine(MontarLabelsHistoriaEfeitos(true));
                    LivroJogo.INSTANCIA.AdicionarEmJogadorEfeitosAplicados(PaginaExecutoraAtual().ObterHistoriaEfeitosAtuais().efeitos);
                    PaginaExecutoraAtual().ImporHistoriaEfeitosExeProcessoEfeito(PROCESSO.PROCESSANDO);
                    LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                    return true;
                }
                else {
                    StartCoroutine(MontarLabelsHistoriaEfeitos(false));
                    PaginaExecutoraAtual().ImporHistoriaEfeitosExeProcessoEfeito(PROCESSO.CONCLUIDO);
                    LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                    return true;
                }
            case PROCESSO.CONCLUIDO:
                PaginaExecutoraAtual().ImporHistoriaEfeitosExeProcessoEfeito(PROCESSO.DESTRUIDO);
                PaginaExecutoraAtual().historiaProcessoIndice = PROCESSO_HISTORIA.PROCESSANDO_IMAGENS;
                LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                return true;
        }
        return false;
    }


    bool AoNotificar_ProcessarTelaHistoria_ProcessandoImagens() {
        if ((PaginaExecutoraAtual().ObterHistoriaImagensAtuais() is null) || (string.IsNullOrWhiteSpace(PaginaExecutoraAtual().ObterHistoriaImagensAtuais().arquivo))) {
            PaginaExecutoraAtual().historiaProcessoIndice = PROCESSO_HISTORIA.CONCLUIDO;
            LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
            return true;
        }
        switch (PaginaExecutoraAtual().ObterHistoriaImagensAtuais().exeProcessoImagem) {
            case PROCESSO.ZERO:
                PaginaExecutoraAtual().ImporHistoriaImagensExeProcessoImagem(PROCESSO.INICIANDO);
                LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                return true;
            case PROCESSO.INICIANDO:
                if (!LivroJogo.INSTANCIA.ehJogoCarregado)
                    StartCoroutine(MontarLabelsHistoriaImagens(false));
                else
                    StartCoroutine(MontarLabelsHistoriaImagens(true));
                LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                return true;
            case PROCESSO.CONCLUIDO:
                PaginaExecutoraAtual().ImporHistoriaEfeitosExeProcessoEfeito(PROCESSO.DESTRUIDO);
                PaginaExecutoraAtual().historiaProcessoIndice = PROCESSO_HISTORIA.CONCLUIDO;
                LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
                return true;
        }
        return false;
    }


    IEnumerator MontarLabelsHistoriaTextos() {
        yield return null;
        foreach (string _textosHistoriaI in PaginaExecutoraAtual().ObterHistoriaTextosAtuais().textosHistoria) {
            Label _label = new Label();
            _label.AddToClassList("historiaTexto");
            _label.text = "";
            historiaGroupBox.Add(_label);
            yield return StartCoroutine(DatilografarTextoAsync(_label, _textosHistoriaI, livroJogoMotor.historiaVelocidadeDoTexto));
        }
        PaginaExecutoraAtual().ObterHistoriaTextosAtuais().exeProcessoTexto = PROCESSO.CONCLUIDO;
        LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
    }


    IEnumerator DatilografarTextoAsync(Label label, string texto, float historiaVelocidadeTexto) {
        label.text = "";
        foreach (char _letraI in texto) {
            label.text += _letraI;
            yield return new WaitForSeconds(historiaVelocidadeTexto);
        }
    }


    void PularHistoria(ClickEvent evento) {
        livroJogoMotor.historiaVelocidadeDoTexto = Constantes.HISTORIA_VELOCIDADE_TEXTO_RAPIDO;
        comandosGroupBox.style.visibility = Uteis.ObterVisibility(false);
    }


    IEnumerator MontarLabelsHistoriaEfeitos(bool aguardaAnimacao) {
        yield return null;
        foreach (EfeitoExecucao _efeitoI in PaginaExecutoraAtual().ObterHistoriaEfeitosAtuais().efeitos) {
            Label _label = new Label();
            if (_efeitoI.quantidade >= 1)
                _label.AddToClassList("historiaEfeitoBom");
            else
                _label.AddToClassList("historiaEfeitoRuim");
            _label.text = _efeitoI.textoEfeito;
            historiaGroupBox.Add(_label);
        }
        if (aguardaAnimacao)
            yield return new WaitForSeconds(Constantes.TEMPO_ANIMACAO_NORMAL);
        PaginaExecutoraAtual().ObterHistoriaEfeitosAtuais().exeProcessoEfeito = PROCESSO.CONCLUIDO;
        LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
    }


    IEnumerator MontarLabelsHistoriaImagens(bool aguardaAnimacao) {
        yield return null;
        if (!File.Exists(PaginaExecutoraAtual().ObterHistoriaImagensAtuais().arquivo)) {
            PaginaExecutoraAtual().ObterHistoriaImagensAtuais().exeProcessoImagem = PROCESSO.CONCLUIDO;
            LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
            yield break;
        }
        Task<Texture2D> _textura = Uteis.CarregarImagemAsync(PaginaExecutoraAtual().ObterHistoriaImagensAtuais().arquivo);
        while (!_textura.IsCompleted)
            yield return null;
        if (_textura.Result is null) {
            PaginaExecutoraAtual().ObterHistoriaImagensAtuais().exeProcessoImagem = PROCESSO.CONCLUIDO;
            LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
            yield break;
        }
        Image _imagem = new Image();
        _imagem.image = _textura.Result;
        _imagem.scaleMode = ScaleMode.ScaleToFit;
        //_imagem.style.width = 200;
        //_imagem.style.height = 200;
        _imagem.AddToClassList("historiaImagem");
        historiaGroupBox.Add(_imagem);
        if (aguardaAnimacao)
            yield return new WaitForSeconds(Constantes.TEMPO_ANIMACAO_NORMAL);
        PaginaExecutoraAtual().ObterHistoriaImagensAtuais().exeProcessoImagem = PROCESSO.CONCLUIDO;
        LivroJogo.INSTANCIA.observadorAlvo_PaginaExecutora.Notificar(OBSERVADOR_CONDICAO.PAGINA_EXECUTORA);
    }
}