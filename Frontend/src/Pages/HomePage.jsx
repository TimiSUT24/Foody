import {useState,useEffect, useRef} from 'react'
import {ProductService} from "../Services/ProductService"
import {useProductFilters} from "../Hooks/useProductFilters";
import ProductFilters from "../Components/ProductFilters";
import ProductCard from "../Components/ProductCard"
import { useSearchParams } from 'react-router-dom';
import "../CSS/HomePage.css"
import "../CSS/ProductCard.css"

export default function HomePage(){
    const [products, setProducts] = useState([]);
    const [searchParams] = useSearchParams();
    const [hasMore, setHasMore] = useState(true);
    const [loading, setLoading] = useState(false);
    const [page, setPage] = useState(1);
    const pageSize = 25;
    const loadMoreRef = useRef(null); // ref does not trigger re-renders

    const isResettingRef = useRef(false);
    const isInitialLoadRef = useRef(true);

    
    const {
        filters,
        updateFilter,
    } = useProductFilters();

    const categoryIdParam = searchParams.get("categoryId");
    const subCategoryIdParam = searchParams.get("subCategoryId");
    const subSubCategoryIdParam = searchParams.get("subSubCategoryId");

    useEffect(() => {
    if (!categoryIdParam) return;

    updateFilter({ categoryId: categoryIdParam ?Number(categoryIdParam) : null,
        subCategoryId: subCategoryIdParam ? Number(subCategoryIdParam) : null,
        subSubCategoryId: subSubCategoryIdParam ? Number(subSubCategoryIdParam) : null
     });
}, [categoryIdParam,subCategoryIdParam,subSubCategoryIdParam]);

useEffect(() => {
    
        isResettingRef.current = true; // access ref value 
        isInitialLoadRef.current = true;

        setProducts([]);
        setPage(1);
        setHasMore(true);
    }, [
        filters.categoryId,
        filters.subCategoryId,
        filters.subSubCategoryId
    ]);

     useEffect(() => {
        if (page > 1 &&!hasMore) return;

        const controller = new AbortController();
        setLoading(true);

        ProductService.getProducts(
            { ...filters, page, pageSize },
            controller.signal // allow request cancellation 
        )
            .then(res => {
                setProducts(prev =>
                    page === 1 ? res.items : [...prev, ...res.items]
                );
                setHasMore(res.hasMore);

                if(page === 1){
                    isResettingRef.current = false;
                    isInitialLoadRef.current = false;
                }
            })
            .catch(err => {
                if (err.name !== "AbortError") {
                    console.error(err);
                }
            })
            .finally(() => setLoading(false));

        return () => controller.abort(); //abort an async operation before it has completed
    }, [filters, page]);


    useEffect(() => {
        if (!hasMore || loading) return;

        const observer = new IntersectionObserver(
            ([entry]) => {
                if (entry.isIntersecting && !isResettingRef.current && !isInitialLoadRef.current) {
                    setPage(prev => prev + 1);
                }
            },
            { rootMargin: "200px" }
        );

        if (loadMoreRef.current) {
            observer.observe(loadMoreRef.current);
        }

        return () => observer.disconnect();
    }, [hasMore, loading]);
    
    
    return (
        <div className ="home-container">
            <div className="food-container">
                <h2 id="food-container-h2"style={{position:"absolute",top:250,placeSelf:"center",color:"white",zIndex:1}}>Premiumlivsmedel, levererade färska</h2>
                <p style={{position:"absolute",top:300,placeSelf:"center",color:"white",zIndex:1}}>Upptäck utsökta köttdetaljer, hantverksbakat bröd och den allra färskaste havsmaten — direkt från noggrant utvalda kvalitetsleverantörer.</p>
                <div style={{position:"absolute",backgroundColor:"black",top:150,width:"100vw",height:400,zIndex:0,opacity:"55%" }}></div>
                <img src="IMG/food.jpg" alt="" className="food-pic" />
            </div>
            <ProductFilters
            filters={filters}
            updateFilter={updateFilter}>
            </ProductFilters>


            <div className ="product-grid">
                {products.map(p => (                   
                    <ProductCard key={p.id} product={p}></ProductCard>         
                ))}
            </div>
            <div ref={loadMoreRef} />

            
            {loading && (
                <p style={{ textAlign: "center", margin: "2rem 0" }}>
                    Loading…
                </p>
            )}
            
        </div>
    )

}