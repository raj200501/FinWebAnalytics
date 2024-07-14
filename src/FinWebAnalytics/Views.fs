module Views

open Giraffe.ViewEngine

let index =
    html [] [
        head [] [
            title [] [ str "FinWebAnalytics" ]
            link [ _rel "stylesheet"; _type "text/css"; _href "/Styles.css" ]
        ]
        body [] [
            h1 [] [ str "Welcome to FinWebAnalytics" ]
            form [ _action "/analyze"; _method "post" ] [
                input [ _type "text"; _name "data"; _placeholder "Enter financial data (comma-separated values)" ]
                button [ _type "submit" ] [ str "Analyze" ]
            ]
            form [ _action "/predict"; _method "post" ] [
                input [ _type "text"; _name "data"; _placeholder "Enter financial data (comma-separated values)" ]
                button [ _type "submit" ] [ str "Predict" ]
            ]
        ]
    ]
