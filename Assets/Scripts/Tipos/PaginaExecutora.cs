using Assets.Scripts.LIB;
using Assets.Scripts.Tipos;
using Newtonsoft.Json;
using System;
using Unity.Jobs;
using UnityEngine;
using static Assets.Scripts.Tipos.Conjuntos;


namespace Assets.Scripts.Tipos {

    [System.Serializable]
    public class PaginaExecutora : Pagina {

        public PAGINA_EXECUTOR_ESTADO paginaEstado = PAGINA_EXECUTOR_ESTADO.NULO;

        public int paginaIdPaginaDestino;

        public CAMPANHA_CAPITULO paginaIdCapituloDestino;

        public ProcessoMotorIEnumerator historiaProcesso = new ProcessoMotorIEnumerator();

        public HistoriaTextoExecucao[] historiaTextos;

        public HistoriaEfeitoExecucao[] historiaEfeitos;

        public HistoriaImagemExecucao[] historiaImagens;

        public int historiaIndice;

        public ProcessoMotorIEnumerator historiaProcessoIndice = new ProcessoMotorIEnumerator();

        public PROCESSO2 combateProcesso = PROCESSO2.ZERO;

        public InimigoExecucao[] combateInimigos;

        public POSTURA_INIMIGO[] combateInimigos_PosturaInimigo;

        public PROCESSO2[] combateInimigos_ProcessoRolagemAtaque;

        public PROCESSO2[] combateInimigos_ProcessoRolagemSorteConfirmacao;

        public EfeitoInimigoExecucao[] combateInimigosEfeitosAplicados;

        public AliadoExecucao combateAliado;

        public EfeitoExecucao[] combateAliadoEfeitosAplicados;

        public string[] combateTextosDerrota;

        public string combateAprovacaoDerrota;

        public bool combateMultiplo_2osApoio;

        public int combateSerieDeAtaqueAtual;

        public PROCESSO2 combateProcessoSerieDeAtaque;

        public RESULTADO_COMBATE combateResultadoFinalDerrota;

        public RESULTADO_COMBATE combateResultadoFinalInimigos;

        public int combateIdPaginaDestinoDerrota;

        //combateDadosJogadorRef: MutableRefObject<DieContainerRef | null>[];

        //combateDadosInimigoRef: MutableRefObject<DieContainerRef | null>[];

        //combateDadosSorteRef: MutableRefObject<DieContainerRef | null>[];

        public PROCESSO2 destinoProcesso = PROCESSO2.ZERO;

        public DestinoExecucao[] destinoItens;

        public PROCESSO2 destinoProcessoRolagem;

        public int destinoRolagemTotal;

        public Destino destinoRolagemDestino;

        //public destinoDadosRef: MutableRefObject<DieContainerRef | null>;

        public bool destinoDesativaBotoes = false;

        public PROCESSO2 destinoProcessoSalvando = PROCESSO2.ZERO;

        public PROCESSO2 destinoProcessoCurando = PROCESSO2.ZERO;



        public PaginaExecutora(Pagina pagina) {
            if (!Pagina.EhValido(pagina))
                return;
            Debug.Log("PaginaExecutadora 0 : " + JsonConvert.SerializeObject(pagina));
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
                    combateInimigos_ProcessoRolagemAtaque = Uteis.AdicionarNoArray<PROCESSO2>(combateInimigos_ProcessoRolagemAtaque, PROCESSO2.ZERO);
                    combateInimigos_ProcessoRolagemSorteConfirmacao = Uteis.AdicionarNoArray<PROCESSO2>(combateInimigos_ProcessoRolagemSorteConfirmacao, PROCESSO2.ZERO);
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
            Debug.Log("PaginaExecutadora 1 : " + JsonConvert.SerializeObject(this));
        }


        public static bool EhValido(PaginaExecutora paginaExecutora) {
            if (paginaExecutora is null)
                return false;
            if (!Pagina.EhValido(paginaExecutora))
                return false;
            return true;
        }


        public HistoriaTextoExecucao ObterHistoriaTextosAtuais() {
            if (historiaTextos is null)
                return null;
            if (historiaTextos.Length <= historiaIndice)
                return null;
            return historiaTextos[historiaIndice];
        }


        public HistoriaEfeitoExecucao ObterHistoriaEfeitosAtuais() {
            if (historiaEfeitos is null)
                return null;
            if (historiaEfeitos.Length <= historiaIndice)
                return null;
            return historiaEfeitos[historiaIndice];
        }


        public HistoriaImagemExecucao ObterHistoriaImagensAtuais() {
            if (historiaImagens is null)
                return null;
            if (historiaImagens.Length <= historiaIndice)
                return null;
            return historiaImagens[historiaIndice];
        }


        public void ImporHistoriaTextosExeProcessoTexto(PROCESSO processo, bool ehProcessamento) {
            if ((historiaTextos != null) && (ObterHistoriaTextosAtuais() != null)) {
                if (ehProcessamento)
                    ObterHistoriaTextosAtuais().exeProcessoTexto.Processar(processo);
                else
                    ObterHistoriaTextosAtuais().exeProcessoTexto.ImporProcesso(processo);
            }
        }


        public void ImporHistoriaEfeitosExeProcessoEfeito(PROCESSO processo, bool ehProcessamento) {
            if ((historiaEfeitos != null) && (ObterHistoriaEfeitosAtuais() != null)) {
                if (ehProcessamento)
                    ObterHistoriaEfeitosAtuais().exeProcessoEfeito.Processar(processo);
                else
                    ObterHistoriaEfeitosAtuais().exeProcessoEfeito.ImporProcesso(processo);
            }
        }


        public void ImporHistoriaImagensExeProcessoImagem(PROCESSO processo, bool ehProcessamento) {
            if ((historiaImagens != null) && (ObterHistoriaImagensAtuais() != null)) {
                if (ehProcessamento)
                    ObterHistoriaImagensAtuais().exeProcessoImagem.Processar(processo);
                else
                    ObterHistoriaImagensAtuais().exeProcessoImagem.ImporProcesso(processo);
            }
        }
    }
}
