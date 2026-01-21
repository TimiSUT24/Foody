import {useState,useEffect} from 'react'
import {ProductService} from "../Services/ProductService"
import { CategoryService } from '../Services/CategoryService'
import {useParams, Link} from 'react-router-dom'
import {useCart} from '../Context/CartContext'
import { CiCirclePlus } from "react-icons/ci";
import { CiCircleMinus } from "react-icons/ci";
import { MdArrowForwardIos } from "react-icons/md";
import "../CSS/ProductDetails.css"

export default function DetailsPage(){
    const { id } = useParams();
    const [productdetails, setProduct] = useState(null);
    const {addToCart,removeFromCart, getQty} = useCart();
    const productId = productdetails?.product?.id;
    const quantity = productId ? getQty(productId) : 0;
    const [category, setCategory] = useState(null);
    const [subCategory, setSubCategory] = useState(null);
    const [subSubCategory, setSubSubCategory] = useState(null);

    useEffect(() => {
        if(id){
            ProductService.getProductDetails(id)
            .then(setProduct)
            .catch((err) => console.error(err));
        }
    }, [id])

    useEffect(() =>{
        if(productdetails?.product?.categoryId){
        CategoryService.getCategoryDetails(productdetails.product.categoryId)
        
        .then(setCategory)
        .catch((err) => console.error(err));
        }

        if(productdetails?.product?.subCategoryId){
        CategoryService.getSubCategoryDetails(productdetails.product.subCategoryId)
        .then(setSubCategory)
        .catch((err) => console.error(err));
        }

        if(productdetails?.product?.subSubCategoryId){
        CategoryService.getSubSubCategoryDetails(productdetails.product.subSubCategoryId)
        .then(setSubSubCategory)
        .catch((err) => console.error(err));
        }
    },[productdetails?.product?.categoryId,productdetails?.product?.subCategoryId,productdetails?.product?.subSubCategoryId])

    if(!productdetails || !productdetails.product){
        return <p>Laddar produkt...</p>
    }

    return (
        <div className="product-details">
            <div className="category-breadcrumb">
                <nav className="breadcrumb">
                    <Link to={`/?categoryId=${category?.id}`}>
                        {category?.mainCategory}
                    </Link>
                    <MdArrowForwardIos className="breadcrumb-arrow"/>
                    <Link to={`/?categoryId=${category?.id}&subCategoryId=${subCategory?.id}`}>
                    {subCategory?.name}</Link>
                    <MdArrowForwardIos className="breadcrumb-arrow"/>
                    <Link
                        to={`/?categoryId=${category?.id}&subCategoryId=${subCategory?.id}&subSubCategoryId=${subSubCategory?.id}`}
                    >
                        {subSubCategory?.name}
                    </Link>
                </nav>
            </div>
            <img className="details-img" src={productdetails.product.imageUrl} alt={productdetails.product.name} />
            <div className="product-info">
            {productdetails.product.brand && <p className="product-brand">Märke: {productdetails.product.brand}</p>}
            <h1>{productdetails.product.name}</h1>
            <div className="product-attribute">
                {productdetails.attribute.map(p => (
                    <p key={p.id}>{p.value}</p>
                ))}
            </div>

            <hr style={{width:"100%",borderStyle:"solid",borderColor:"gray",opacity:"15%"}}/>
            <div id="product-info-price">
                {productdetails.product.hasOffer ? (
                        <div >
                            <span style={{ textDecoration: "line-through", opacity: 0.6, paddingLeft:20, fontSize:18 }}>
                                {productdetails.product.price} {productdetails.product.currency}
                            </span>
                            <span style={{ color: "red", marginLeft: 8, fontWeight:"bold", fontSize:18 }}>
                                {productdetails.product.finalPrice} {productdetails.product.currency} {productdetails.product.offerName}
                            </span>
                        </div>
                    ) : (
                        <p style={{fontWeight:"bold",fontSize:18}}>{productdetails.product.price}</p>
                    )}
                <p>{productdetails.product.comparePrice}</p>    
            </div>
            <p id="weightText">{productdetails.product.weightText}</p>  
             <hr style={{width:"100%",borderStyle:"solid",borderColor:"gray",opacity:"15%"}}/>
            {quantity === 0 ? (
                                    <button id="add-details-button" onClick={() => addToCart(productdetails.product)}>Lägg till</button>
                                    ) : (
                                        <div style={{display:"flex",gap:10,justifyContent:"center",height:47}}>
                                    <button id="minus-details-button" onClick={() => removeFromCart(productdetails.product.id)} style={{width:100}}><CiCircleMinus style={{width:22,height:22}}/></button>
                                    <p style={{marginTop:20}}>{quantity} st</p>
                                    <button id="add-details-button" onClick={() => addToCart(productdetails.product)} style={{width:100}}><CiCirclePlus style={{width:22,height:22,color:"white"}}/></button>
                                </div>
                                )}
            </div>

            <div className="product-extra-info">
            {/*if empty or null dont show p tags */}
            {productdetails.product.productInformation && <p><strong>ProduktInformation:</strong> {productdetails.product.productInformation}</p>}
            {productdetails.product.country && <p style={{display:"flex", flexDirection:"column", gap:5}}><strong>Land:</strong> {productdetails.product.country}</p>}
            {productdetails.product.usage && <p style={{display:"flex", flexDirection:"column", gap:5}}><strong>Användning:</strong> {productdetails.product.usage}</p>}
            {productdetails.product.allergens && <p style={{display:"flex", flexDirection:"column", gap:5}}><strong>Allergener:</strong> {productdetails.product.allergens}</p>}
            {productdetails.product.storage && <p style={{display:"flex", flexDirection:"column", gap:5}}><strong>Förvaring:</strong> {productdetails.product.storage}</p>}

                {productdetails.nutrition?.length > 0 && (
                    <table className="product-nutrition"> 
                    <thead>
                    <tr>
                        <th colSpan="2"><h2 style={{textAlign:"left"}}>Näringsvärde</h2></th>
                    </tr>
                    </thead>

                    <thead>
                        <tr>
                            <th style={{display:"flex"}}>
                                {productdetails.nutrition[0].nutritionUnitText}
                            </th>
                            <th>Value</th>
                        </tr>
                        
                        </thead>                
                <tbody className="nutrition-table">
                {productdetails.nutrition.map(p => (
                    <tr key={p.id} className="tes" style={{backgroundColor:"whitesmoke"}}>
                        
                        <td className="label" style={{display:"flex",marginRight:100,padding:7}}>{p.name}</td>
                        <td className="value" >{p.value}</td>                    
                    </tr>      
                ))}
                </tbody>
                </table>
                )}

                {productdetails.product.ingredients && <p style={{backgroundColor:"white",borderRadius:5,width:630,marginLeft:20,padding:10}}><strong>Ingredienser:</strong> {productdetails.product.ingredients}</p>}
            </div>
        </div>
    )

}

