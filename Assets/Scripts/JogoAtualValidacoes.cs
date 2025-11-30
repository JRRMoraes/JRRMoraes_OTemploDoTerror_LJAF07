using Assets.Scripts.Tipos;
using System.Linq;
using UnityEngine;
using static Assets.Scripts.Tipos.Conjuntos;


namespace Assets.Scripts {

    public class JogoAtualValidacoes {

        static Jogo ObterJogoAtual() {
            return LivroJogo.INSTANCIA.jogoAtual;
        }


        static PaginaExecutora ObterPaginaExecutora() {
            return LivroJogo.INSTANCIA.paginaExecutora;
        }


        public static bool ValidarAprovacoesDestino(AprovacaoDestino[] aprovacoes) {
            if ((aprovacoes is null) || (aprovacoes.Length <= 0))
                return true;
            bool _ok = true;
            foreach (AprovacaoDestino _aprovacaoI in aprovacoes) {
                switch (_aprovacaoI.atributoAprovacao) {
                    case ATRIBUTO.FUNCAO:
                        _ok &= ValidarAprovacoesDestino_Funcao(_aprovacaoI);
                        break;
                    case ATRIBUTO.ITENS:
                        _ok &= ValidarAprovacoesDestino_Itens(_aprovacaoI);
                        break;
                    case ATRIBUTO.ENCANTOS:
                        _ok &= ValidarAprovacoesDestino_Encantos(_aprovacaoI);
                        break;
                    default:
                        Debug.Log($"@ OperacoesJogoLivro:ValidarAprovacoesDestino:: Não foi encontrado o atributo {_aprovacaoI.atributoAprovacao.ToString()} com nome '{_aprovacaoI.nomeAprovacao}'.");
                        _ok = false;
                        break;
                }
            }
            return _ok;
        }


        static bool ValidarAprovacoesDestino_Funcao(AprovacaoDestino aprovacao) {
            int _quantidade = 0;
            switch (aprovacao.nomeAprovacao) {
                case "JogoAtual_Panilha":
                    _quantidade = ((ObterJogoAtual() != null) && (ObterJogoAtual().panilha != null)) ? 1 : 0;
                    break;
                default:
                    Debug.Log($"@ Operação de destino '{aprovacao.nomeAprovacao}' não foi encontrada");
                    return false;
            }
            return ValidarComparacaoEQuantidade(aprovacao, _quantidade);
        }


        static bool ValidarAprovacoesDestino_Itens(AprovacaoDestino aprovacao) {
            if (!Jogo.EhValido(ObterJogoAtual(), true))
                return ValidarComparacaoEQuantidade(aprovacao, 0);
            if ((ObterJogoAtual().panilha.itens is null) || (ObterJogoAtual().panilha.itens.Length <= 0))
                return ValidarComparacaoEQuantidade(aprovacao, 0);
            if (string.IsNullOrWhiteSpace(aprovacao.nomeAprovacao))
                return ValidarComparacaoEQuantidade(aprovacao, 0);
            Item _item = ObterJogoAtual().panilha.itens.FirstOrDefault((_itemI) => Uteis.TextosIguais(_itemI.idItem, aprovacao.nomeAprovacao, true));
            if (_item != null)
                return ValidarComparacaoEQuantidade(aprovacao, _item.quantidade);
            else
                return ValidarComparacaoEQuantidade(aprovacao, 0);
        }


        static bool ValidarAprovacoesDestino_Encantos(AprovacaoDestino aprovacao) {
            if (!Jogo.EhValido(ObterJogoAtual(), true))
                return ValidarComparacaoEQuantidade(aprovacao, 0);
            if ((ObterJogoAtual().panilha.encantos is null) || (ObterJogoAtual().panilha.encantos.Length <= 0))
                return ValidarComparacaoEQuantidade(aprovacao, 0);
            if (!string.IsNullOrWhiteSpace(aprovacao.nomeAprovacao)) {
                Encanto _encanto = ObterJogoAtual().panilha.encantos.FirstOrDefault((_encantoI) => Uteis.TextosIguais(_encantoI.idEncanto, aprovacao.nomeAprovacao, true));
                if (_encanto != null)
                    return ValidarComparacaoEQuantidade(aprovacao, 1);
                else
                    return ValidarComparacaoEQuantidade(aprovacao, 0);
            }
            else {
                return ValidarComparacaoEQuantidade(aprovacao, ObterJogoAtual().panilha.encantos.Length);
            }
        }


        static bool ValidarComparacaoEQuantidade(AprovacaoDestino aprovacao, int quantidade) {
            switch (aprovacao.comparacao) {
                case COMPARACAO.MAIOR_IGUAL:
                    return quantidade >= aprovacao.quantidade;
                case COMPARACAO.MAIOR:
                    return quantidade > aprovacao.quantidade;
                case COMPARACAO.MENOR_IGUAL:
                    return quantidade <= aprovacao.quantidade;
                case COMPARACAO.MENOR:
                    return quantidade < aprovacao.quantidade;
                case COMPARACAO.NAO_POSSUIR:
                    return quantidade <= 0;
                case COMPARACAO.POSSUIR:
                default:
                    return quantidade >= 1;
            }
        }


        static RESULTADO_COMBATE AvaliarResultadoCombateDoCombateExecutorProcessoIniciando() {
            if (Uteis.TextosIguais(ObterPaginaExecutora().combateAprovacaoDerrota, Constantes.COMBATE_APROVACAO_DERROTA__SERIE_DE_ATAQUE_EH_MAIOR_OU_IGUAL_A_HABILIDADE, true)) {
                if (ObterPaginaExecutora().combateSerieDeAtaqueAtual >= ObterJogoAtual().panilha.habilidade) {
                    ObterPaginaExecutora().combateResultadoFinalDerrota = RESULTADO_COMBATE.DERROTA;
                    return RESULTADO_COMBATE.DERROTA;
                }
            }
            return RESULTADO_COMBATE.COMBATENDO;
        }


        static RESULTADO_COMBATE AvaliarResultadoCombateDoCombateExecutorProcessoDestruido() {
            if (!Jogo.EhValido(ObterJogoAtual(), true))
                return RESULTADO_COMBATE.COMBATENDO;
            if (ObterJogoAtual().panilha.energiaInicial <= 0)
                return RESULTADO_COMBATE.COMBATENDO;
            if (ObterJogoAtual().panilha.energia <= 0) {
                ObterPaginaExecutora().combateResultadoFinalInimigos = RESULTADO_COMBATE.DERROTA;
                return RESULTADO_COMBATE.DERROTA;
            }
            if (!ObterPaginaExecutora().combateInimigos_PosturaInimigo.Any((_posturaInimigoI) => _posturaInimigoI != POSTURA_INIMIGO.MORTO)) {
                ObterPaginaExecutora().combateResultadoFinalInimigos = RESULTADO_COMBATE.VITORIA;
                return RESULTADO_COMBATE.VITORIA;
            }
            if (Uteis.TextosIguais(ObterPaginaExecutora().combateAprovacaoDerrota, Constantes.COMBATE_APROVACAO_DERROTA__INIMIGO_COM_SERIE_DE_ATAQUE_VENCIDO_CONSECUTIVO_2, true)) {
                if (ObterPaginaExecutora().combateInimigos.Any((_inimigoI) => _inimigoI.exeSerieDeAtaqueVencidoConsecutivo >= 2)) {
                    ObterPaginaExecutora().combateResultadoFinalDerrota = RESULTADO_COMBATE.DERROTA;
                    return RESULTADO_COMBATE.DERROTA;
                }
            }
            return RESULTADO_COMBATE.COMBATENDO;
        }


        static string MontarElementoCombateAprovacaoDerrota() {
            return "";
            //switch (combateAprovacaoDerrota.toLowerCase()) {
            //    case COMBATE_APROVACAO_DERROTA__SERIE_DE_ATAQUE_EH_MAIOR_OU_IGUAL_A_HABILIDADE.toLowerCase():
            //        return (
            //            < div className ={ styles.combate_derrota_operacoesJogoLivro + " " + styles.combate_linhaUnica}>
            //                < span className ={ styles.combate_linhaUnica}>
            //                    < span >{ "Habilidade"}</ span >
            //                    < span className ={ styles.combate_derrota_operacoesJogoLivro_numeroAtual}>{ jogoAtual.panilha.habilidade}</ span >
            //                </ span >
            //                < span >{ "<"}</ span >
            //                < span className ={ styles.combate_linhaUnica}>
            //                    < span >{ "Série de ataque"}</ span >
            //                    < span className ={ styles.combate_derrota_operacoesJogoLivro_numeroAtual}>{ combateSerieDeAtaqueAtual}</ span >
            //                </ span >
            //            </ div >
            //            );
            //    default:
            //        return <></>;
            //}
        }


        static bool AprovarExibicaoDeSerieDeAtaqueVencidoConsecutivo() {
            return (!string.IsNullOrWhiteSpace(ObterPaginaExecutora().combateAprovacaoDerrota))
                && (Uteis.TextosIguais(ObterPaginaExecutora().combateAprovacaoDerrota, Constantes.COMBATE_APROVACAO_DERROTA__INIMIGO_COM_SERIE_DE_ATAQUE_VENCIDO_CONSECUTIVO_2, true));
        }
    }
}
