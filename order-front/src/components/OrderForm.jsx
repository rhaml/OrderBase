import {api} from "../services/api"
import {useState} from "react"

export function OrderForm() {
    const [symbol, setSymbol] = useState("PETR4");
    const [side, setSide] = useState("BUY");
    const [quantity, setQuantity] = useState("");
    const [price, setPrice] = useState("");
    const [errorQuantity, setErrorQuantity] = useState('');
    const [errorPrice, setErrorPrice] = useState('');

    const [loading, setLoading] = useState(false);
    const [response, setResponse] = useState(null);
    const [orders, setOrders] = useState([])

    const validateNumber = (value, setValue, setError, max) => {
        if (value <= max) {
            setValue(value);
            setError('');
        } else {
            setError(`O limite máximo é de ${max}.`);
        }
    };

    async function orderSubmit(e) {
        e.preventDefault();

        try{
            setLoading(true)

            const result = await api.post("api/Orders", {symbol, side, quantity:Number(quantity), price:Number(price)})
            setResponse(result.data)
        }
        catch (error) {
            setResponse({
                success: false,
                message: error.response?.data?.message || "Falha ao enviar a ordem!"
            })
        }
        finally {
            setLoading(false)
        }
    }
    return (
        <div>
            <h2>Order Generator</h2>
            <form onSubmit={orderSubmit}>
                <div>
                    <label>Simbolo</label>
                    <select value={symbol} onChange={(e) => setSymbol(e.target.value)}>
                        <option>PETR4</option>
                        <option>VAL3</option>
                        <option>VIIA4</option>
                    </select>
                </div>
                <div>
                    <label>Lado</label>
                    <select value={side} onChange={(e) => setSide(e.target.value)}>
                        <option value="Buy">Compra</option>
                        <option value="Sell">Venda</option>
                    </select>
                </div>
                <div>
                    <label>Quantidade</label>
                    <input type="number" value={quantity} onChange={(e) => validateNumber(e.target.value, setQuantity, setErrorQuantity, 1000000)}></input>
                     {errorQuantity && <p style={{ color: 'red', margin: '5px 0' }}>{errorQuantity}</p>}
                </div>
                <div>
                    <label>Preço</label>
                    <input type="number" step="0.01" value={price} onChange={(e) => validateNumber(e.target.value, setPrice, setErrorPrice, 1000)}></input>
                     {errorPrice && <p style={{ color: 'red', margin: '5px 0' }}>{errorPrice}</p>}
                </div>
                <button type="submit" disabled={loading}>{loading ? "Enviando..." : "Enviar Ordem"}</button>
            </form>
            {response && (
                <div style={{ marginTop: 30 }}>
                    <h3>Resposta</h3>
                    <pre>
                        {JSON.stringify(response, null, 2)}
                    </pre>
                </div>
            )}
        </div>
    )
}