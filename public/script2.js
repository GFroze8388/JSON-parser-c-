let q = [0, 0, 0, 0]
let r = [3, 1, 1, 3]
let rightAnswers = 0
const answers = ['колотые', 'резаные', 'ушибленные', 'рубленные', 'рваные', 'укушенные', 'огнестрельные']
let truth
function a(q2, a){
    if (q[a - 1] == 0) {document.getElementById(`a${(a - 1) * 3 + q2}`).style.backgroundColor = "orange"}
    else {
        document.getElementById(`a${(a - 1) * 3 + q[a - 1]}`).style.backgroundColor = "cadetblue"
        document.getElementById(`a${(a - 1) * 3 + q2}`).style.backgroundColor = "orange"
    }
    q[a - 1] = q2
    
    
}
function check(){
    q.forEach((e, i) => {
        let obj = document.querySelector(`#a${i * 3 + e}`)
        if (e == 0) return
        if (e == r[i]){
            rightAnswers++
            obj.style.backgroundColor = "lightgreen"
        }
        else{
            obj.style.backgroundColor = "red"
        }
    })
    document.getElementById("check").remove()
    let input = document.getElementById('input').value
    for (let i = -1; i < 6; i++){
        if (input == answers[i]){
            truth = true
            rightAnswers++
            break
        }
    }
    if (truth == true){
        document.getElementById('input').style.color = "lightgreen"
    } else{
        document.getElementById('input').style.color = "red"
    }
    document.getElementById("rightAnswers").innerText = `Правильных ответов: ${rightAnswers}`
}
