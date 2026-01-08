namespace FinWebAnalytics

open Giraffe.ViewEngine

module Views =
    let private formRow labelText name placeholder =
        div [ _class "form-row" ] [
            label [] [ str labelText ]
            input [ _type "text"; _name name; _placeholder placeholder ]
        ]

    let index =
        html [] [
            head [] [
                title [] [ str "FinWebAnalytics" ]
                link [ _rel "stylesheet"; _type "text/css"; _href "/styles.css" ]
            ]
            body [] [
                header [] [
                    h1 [] [ str "FinWebAnalytics" ]
                    p [] [ str "Run quick financial analysis and predictive scoring with lightweight analytics." ]
                ]
                main [] [
                    section [] [
                        h2 [] [ str "Analyze data" ]
                        p [] [ str "Enter comma-separated numeric values to compute summary statistics." ]
                        form [ _action "/analyze"; _method "post" ] [
                            formRow "CSV values" "data" "120.5, 121.1, 119.7, 122.0"
                            button [ _type "submit" ] [ str "Analyze" ]
                        ]
                    ]
                    section [] [
                        h2 [] [ str "Predict" ]
                        p [] [ str "Provide two features to generate a deterministic predictive score." ]
                        form [ _action "/predict"; _method "post" ] [
                            formRow "Features" "data" "5.0, 3.2"
                            button [ _type "submit" ] [ str "Predict" ]
                        ]
                    ]
                    section [] [
                        h2 [] [ str "API endpoints" ]
                        ul [] [
                            li [] [ str "POST /analyze (text response)" ]
                            li [] [ str "POST /api/analyze (JSON response)" ]
                            li [] [ str "POST /predict (text response)" ]
                            li [] [ str "POST /api/predict (JSON response)" ]
                            li [] [ str "GET /health" ]
                        ]
                    ]
                ]
                footer [] [
                    p [] [ str "FinWebAnalytics - demo analytics pipeline." ]
                ]
            ]
        ]
