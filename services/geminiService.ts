
import { GoogleGenAI } from "@google/genai";
import { Purchase, BankAccount, CreditCard } from "../types";

const API_KEY = process.env.API_KEY || "";

export const getFinancialAdvice = async (
  accounts: BankAccount[],
  cards: CreditCard[],
  purchases: Purchase[]
) => {
  if (!API_KEY) return "Configure sua API Key para receber conselhos financeiros inteligentes.";

  try {
    const ai = new GoogleGenAI({ apiKey: API_KEY });
    const model = ai.models.generateContent({
      model: "gemini-3-flash-preview",
      contents: `Analise as seguintes finanças e dê 3 dicas curtas e práticas em português:
        Contas: ${JSON.stringify(accounts)}
        Cartões: ${JSON.stringify(cards)}
        Compras recentes: ${JSON.stringify(purchases.slice(-5))}
        Considere o valor total de compras e parcelamentos.`,
    });

    const response = await model;
    return response.text;
  } catch (error) {
    console.error("Gemini Error:", error);
    return "Não foi possível carregar as dicas automáticas no momento.";
  }
};
