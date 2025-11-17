using Assets.Scripts.Tipos;
using System;
using Unity.Jobs;
using UnityEngine;
using static Assets.Scripts.Tipos.Conjuntos;


namespace Assets.Scripts.Tipos {

    public class PaginaExecutora : Pagina {

        public PAGINA_EXECUTOR_ESTADO paginaEstado = PAGINA_EXECUTOR_ESTADO.NULO;

        public int paginaIdPaginaDestino;

        public CAMPANHA_CAPITULO paginaIdCapituloDestino;

        public PROCESSO historiaProcesso = PROCESSO.ZERO;

        public HistoriaTextoExecucao[] historiaTextos;

        public HistoriaEfeitoExecucao[] historiaEfeitos;

        public HistoriaImagemExecucao[] historiaImagens;

        public int historiaIndice;

        public PROCESSO_HISTORIA historiaProcessoIndice = PROCESSO_HISTORIA.ZERO;

        public PROCESSO combateProcesso = PROCESSO.ZERO;

        public InimigoExecucao[] combateInimigos;

        public POSTURA_INIMIGO[] combateInimigos_PosturaInimigo;

        public PROCESSO[] combateInimigos_ProcessoRolagemAtaque;

        public PROCESSO[] combateInimigos_ProcessoRolagemSorteConfirmacao;

        public EfeitoInimigoExecucao[] combateInimigosEfeitosAplicados;

        public AliadoExecucao combateAliado;

        public EfeitoExecucao[] combateAliadoEfeitosAplicados;

        public string[] combateTextosDerrota;

        public string combateAprovacaoDerrota;

        public bool combateMultiplo_2osApoio;

        public int combateSerieDeAtaqueAtual;

        public PROCESSO combateProcessoSerieDeAtaque;

        public RESULTADO_COMBATE combateResultadoFinalDerrota;

        public RESULTADO_COMBATE combateResultadoFinalInimigos;

        public int combateIdPaginaDestinoDerrota;

        //combateDadosJogadorRef: MutableRefObject<DieContainerRef | null>[];

        //combateDadosInimigoRef: MutableRefObject<DieContainerRef | null>[];

        //combateDadosSorteRef: MutableRefObject<DieContainerRef | null>[];

        public PROCESSO destinoProcesso = PROCESSO.ZERO;

        public DestinoExecucao[] destinoItens;

        public PROCESSO destinoProcessoRolagem;

        public int destinoRolagemTotal;

        public Destino destinoRolagemDestino;

        //public destinoDadosRef: MutableRefObject<DieContainerRef | null>;

        public bool destinoDesativaBotoes = false;

        public PROCESSO destinoProcessoSalvando = PROCESSO.ZERO;

        public PROCESSO destinoProcessoCurando = PROCESSO.ZERO;



        public PaginaExecutora(Pagina pagina) {
            if (!Pagina.EhValido(pagina))
                return;
            Debug.Log(pagina);
            idPagina = pagina.idPagina;
            idCapitulo = pagina.idCapitulo;
            titulo = pagina.titulo;
            historias = Uteis.Clonar<Historia[]>(pagina.historias);
            combate = Uteis.Clonar<Combate>(pagina.combate);
            destinos = Uteis.Clonar<Destino[]>(pagina.destinos);
            paginaIdPaginaDestino = PaginaUtils.PAGINA_ZERADA().idPagina;
            paginaIdCapituloDestino = PaginaUtils.PAGINA_ZERADA().idCapitulo;

            if ((historias != null) && (historias.Length > 0)) {
                foreach (Historia _historiaI in historias) {
                    historiaTextos = Uteis.AdicionarNoArray<HistoriaTextoExecucao>(historiaTextos,
                        HistoriaTextoExecucao.CriarCom(_historiaI.textosHistoria));
                    historiaEfeitos = Uteis.AdicionarNoArray<HistoriaEfeitoExecucao>(historiaEfeitos,
                        HistoriaEfeitoExecucao.CriarCom(_historiaI.efeitos));
                    historiaImagens = Uteis.AdicionarNoArray<HistoriaImagemExecucao>(historiaImagens,
                        HistoriaImagemExecucao.CriarCom(_historiaI.imagem));
                }
            }
            else {
                historias = null;
            }

            if ((combate != null) && (combate.inimigos != null) && (combate.inimigos.Length > 0)) {
                int _indiceInimigo = 0;
                foreach (Inimigo _inimigoI in combate.inimigos) {
                    combateInimigos = Uteis.AdicionarNoArray<InimigoExecucao>(combateInimigos, InimigoExecucao.CriarCom(_inimigoI, _indiceInimigo));
                    combateInimigos_PosturaInimigo = Uteis.AdicionarNoArray<POSTURA_INIMIGO>(combateInimigos_PosturaInimigo, POSTURA_INIMIGO.AGUARDANDO);
                    combateInimigos_ProcessoRolagemAtaque = Uteis.AdicionarNoArray<PROCESSO>(combateInimigos_ProcessoRolagemAtaque, PROCESSO.ZERO);
                    combateInimigos_ProcessoRolagemSorteConfirmacao = Uteis.AdicionarNoArray<PROCESSO>(combateInimigos_ProcessoRolagemSorteConfirmacao, PROCESSO.ZERO);
                    combateInimigosEfeitosAplicados = Uteis.AdicionarNoArray<EfeitoInimigoExecucao>(combateInimigosEfeitosAplicados, EfeitoInimigoExecucao.CriarCom(_inimigoI, _indiceInimigo));
                    _indiceInimigo++;
                }
                combateAliado = AliadoExecucao.CriarCom(combate.aliado);
                if ((combate.textosDerrota != null) && (combate.textosDerrota.Length > 0)) {
                    combateTextosDerrota = combate.textosDerrota;
                    combateResultadoFinalDerrota = RESULTADO_COMBATE.COMBATENDO;
                }
                if (!string.IsNullOrWhiteSpace(combate.aprovacaoDerrota)) {
                    combateAprovacaoDerrota = combate.aprovacaoDerrota;
                    combateResultadoFinalDerrota = RESULTADO_COMBATE.COMBATENDO;
                }
                combateMultiplo_2osApoio = combate.combateMultiplo_2osApoio;
                combateIdPaginaDestinoDerrota = PaginaUtils.PAGINA_ZERADA().idPagina;
            }
            else {
                combate = null;
            }

            if ((destinos != null) && (destinos.Length > 0)) {
                foreach (Destino _destinoI in destinos)
                    destinoItens = Uteis.AdicionarNoArray<DestinoExecucao>(destinoItens, DestinoExecucao.CriarCom(_destinoI));
            }
            else {
                destinos = null;
            }

            //if (combate != null) {
            //    ImporAudioMusicaViaMomento(EAudioMomentoMusica.COMBATE);
            //}
            //else {
            //    ImporAudioMusicaViaMomento(EAudioMomentoMusica.CAMPANHA);
            //}
            Debug.Log(this);
        }


        public static bool EhValido(PaginaExecutora paginaExecutora) {
            if (paginaExecutora is null)
                return false;
            if (!Pagina.EhValido(paginaExecutora))
                return false;
            return true;
        }


        public HistoriaTextoExecucao ObterHistoriaTextosAtuais() {
            if (historiaTextos.Length <= historiaIndice)
                return null;
            return historiaTextos[historiaIndice];
        }


        public HistoriaEfeitoExecucao ObterHistoriaEfeitosAtuais() {
            if (historiaEfeitos.Length <= historiaIndice)
                return null;
            return historiaEfeitos[historiaIndice];
        }


        public HistoriaImagemExecucao ObterHistoriaImagensAtuais() {
            if (historiaImagens.Length <= historiaIndice)
                return null;
            return historiaImagens[historiaIndice];
        }


        public void ImporHistoriaTextosExeProcessoTexto(PROCESSO processo) {
            if ((historiaTextos != null) && (ObterHistoriaTextosAtuais() != null) && (ObterHistoriaTextosAtuais().exeProcessoTexto != processo))
                ObterHistoriaTextosAtuais().exeProcessoTexto = processo;
        }


        public void ImporHistoriaEfeitosExeProcessoEfeito(PROCESSO processo) {
            if ((historiaEfeitos != null) && (ObterHistoriaEfeitosAtuais() != null) && (ObterHistoriaEfeitosAtuais().exeProcessoEfeito != processo))
                ObterHistoriaEfeitosAtuais().exeProcessoEfeito = processo;
        }


        public void ImporHistoriaImagensExeProcessoImagem(PROCESSO processo) {
            if ((historiaImagens != null) && (ObterHistoriaImagensAtuais() != null) && (ObterHistoriaImagensAtuais().exeProcessoImagem != processo))
                ObterHistoriaImagensAtuais().exeProcessoImagem = processo;
        }
    }
}
