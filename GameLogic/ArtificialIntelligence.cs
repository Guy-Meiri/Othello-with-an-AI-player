using System;

namespace GameLogicUnit
{
    public class ArtificialIntelligence
    {
        private const int k_SearchMaximalDepth = 4;
        private const int k_BonusForCornerMove = 4;

        public static PlayerMove MiniMaxChooseMove(GameState i_GameState)
        {
            PlayerMove chosenMove = new PlayerMove();
            GameLogic gameNode = new GameLogic(i_GameState);

            int maxGuaranteedValue = miniMaxChooseMoveRecursive(gameNode, k_SearchMaximalDepth, ref chosenMove, int.MinValue, int.MaxValue);

            return chosenMove;
        }

        private static int miniMaxChooseMoveRecursive(GameLogic i_GameNode, int i_Depth, ref PlayerMove io_ChosenMove, int i_Alpha, int i_Beta)
        {
            int evaluationOfAllChildrenNodes = 0;
            bool isMaximizingPlayer;

            i_GameNode.ChangeToNextPlayerWithLegalMoves();

            if (i_Depth == 0 || i_GameNode.CurrentGameState.IsGameOver)
            {
                evaluationOfAllChildrenNodes = i_GameNode.CurrentGameState.EvaluatePlayerScoreDifference();
            }
            else
            {
                isMaximizingPlayer = i_GameNode.CurrentGameState.CurrentPlayer.PlayerType == Player.ePlayerType.Computer;

                if (isMaximizingPlayer)
                {
                    evaluationOfAllChildrenNodes = makeMaximizingMove(i_GameNode, i_Depth, ref io_ChosenMove, i_Alpha, i_Beta);
                }
                else
                {
                    evaluationOfAllChildrenNodes = makeMinimizingMove(i_GameNode, i_Depth, ref io_ChosenMove, i_Alpha, i_Beta);
                }
            }

            return evaluationOfAllChildrenNodes;
        }

        private static int getBonusForMove(GameState i_GameState, PlayerMove i_PlayerMove)
        {
            int bonus = 0;

            if (i_GameState.GameBoard.IsCellInCorners(i_PlayerMove.Row, i_PlayerMove.Column))
            {
                Random randomNumberGenerator = new Random();
                bonus = k_BonusForCornerMove + randomNumberGenerator.Next(0, 2);
            }

            return bonus;
        }

        private static int makeMinimizingMove(GameLogic i_GameNode, int i_Depth, ref PlayerMove io_ChosenMove, int i_Alpha, int i_Beta)
        {
            int minEvaluation = int.MaxValue;
            int currentEvaluation = int.MaxValue;
            PlayerMove moveFromRecursion = new PlayerMove();

            foreach (PlayerMove currentMove in i_GameNode.PossibleMovesList)
            {
                GameLogic nextMoveGameNode = new GameLogic(i_GameNode.CurrentGameState);
                nextMoveGameNode.UpdateGamestate(currentMove);
                nextMoveGameNode.CurrentGameState.ChangePlayerTurn();
                currentEvaluation = miniMaxChooseMoveRecursive(nextMoveGameNode, i_Depth - 1, ref moveFromRecursion, i_Alpha, i_Beta);

                if (currentEvaluation < minEvaluation)
                {
                    minEvaluation = currentEvaluation;
                    io_ChosenMove = currentMove;
                }

                i_Beta = Math.Min(i_Beta, currentEvaluation);

                if (i_Beta <= i_Alpha)
                {
                    break;
                }
            }

            return minEvaluation;
        }

        private static int makeMaximizingMove(GameLogic i_GameNode, int i_Depth, ref PlayerMove io_ChosenMove, int i_Alpha, int i_Beta)
        {
            int maxEvaluation = int.MinValue;
            int currentEvaluation = int.MinValue;
            PlayerMove moveFromRecursion = new PlayerMove();

            foreach (PlayerMove currentMove in i_GameNode.PossibleMovesList)
            {
                GameLogic nextMoveGameNode = new GameLogic(i_GameNode.CurrentGameState);
                nextMoveGameNode.UpdateGamestate(currentMove);
                nextMoveGameNode.CurrentGameState.ChangePlayerTurn();
                currentEvaluation = miniMaxChooseMoveRecursive(nextMoveGameNode, i_Depth - 1, ref moveFromRecursion, i_Alpha, i_Beta);
                currentEvaluation += getBonusForMove(nextMoveGameNode.CurrentGameState, currentMove);

                if (currentEvaluation > maxEvaluation)
                {
                    maxEvaluation = currentEvaluation;
                    io_ChosenMove = currentMove;
                }

                i_Alpha = Math.Max(i_Alpha, currentEvaluation);

                if (i_Beta <= i_Alpha)
                {
                    break;
                }
            }

            return maxEvaluation;
        }
    }
}