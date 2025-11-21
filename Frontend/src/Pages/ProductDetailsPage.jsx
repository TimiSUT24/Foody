import {useState,useEffect} from 'react'
import {ProductService} from "../Services/ProductService"
import {useParams} from 'react-router-dom'
import {useCart} from '../Context/CartContext'

export default function DetailsPage(){
    const { id } = useParams();
    const [productdetails, setProduct] = useState(null);
    const {addToCart} = useCart();

    useEffect(() => {
        if(id){
            ProductService.getProductDetails(id)
            .then(setProduct)
            .catch((err) => console.error(err));
        }
    }, [id])

    if(!productdetails){
        return <p>Laddar produkt...</p>
    }

    return (
        <div className="product-details">
            <img src={productdetails.product.imageUrl} alt={productdetails.product.name} />
            <div className="product-info">
            <h1>{productdetails.product.name}</h1>
            <p>{productdetails.product.weightText}</p>
            <p>{productdetails.product.comparePrice}</p>
            <p>{productdetails.product.price} {productdetails.product.currency}</p>
            <button onClick={() => addToCart(productdetails.product)}>Lägg till</button>
            </div>

            <div className="product-attribute">
                {productdetails.attribute.map(p => (
                    <p key={p.id}>{p.value}</p>
                ))}
            </div>

            <div className="product-extra-info">

            {/*if empty or null dont show p tags */}
            {productdetails.product.productInformation && <p>{productdetails.product.productInformation}</p>}
            {productdetails.product.country && <p>Land: {productdetails.product.country}</p>}
            {productdetails.product.brand && <p>Märke: {productdetails.product.brand}</p>}
            {productdetails.product.usage && <p>Användning: {productdetails.product.usage}</p>}
            {productdetails.product.allergens && <p>Allergener: {productdetails.product.allergens}</p>}
            {productdetails.product.ingredients && <p>Ingredienser: {productdetails.product.ingredients}</p>}
            {productdetails.product.storage && <p>Förvaring: {productdetails.product.storage}</p>}

                {productdetails.nutrition?.length > 0 && (
                    <div className="product-nutrition">
                    <h2>Näringsvärde</h2>
                <ul>
                {productdetails.nutrition.map(p => (
                    <li key={p.id}>{p.name}: {p.value} </li>
                ))}
                </ul>
                </div>
                )}
            </div>
        </div>
    )

}

