import React, { Component, createRef} from 'react';
import parse from 'html-react-parser'

export class Hospitals extends Component {
  static displayName = Hospitals.name;

  constructor(props) {
    super(props);
      this.state = {
          hospitals: [],
          loading: true,
          showEdit: false
      };

      this.hospitalId = createRef();
      this.name = createRef();
      this.description = createRef();
      this.addressLine1 = createRef();
      this.addressLine2 = createRef();
      this.addressLine3 = createRef();
      this.addressCity = createRef();
      this.addressState = createRef();
      this.addressZip = createRef();
      this.phoneNumber = createRef();
    }

  componentDidMount() {
    this.populateHospitalData();
    }

    resetForm() {
        this.hospitalId.current = null;
        this.name.current.value = null;
        this.description.current.value = null;
        this.addressLine1.current.value = null;
        this.addressLine2.current.value = null;
        this.addressLine3.current.value = null;
        this.addressCity.current.value = null;
        this.addressState.current.value = null;
        this.addressZip.current.value = null;
        this.phoneNumber.current.value = null;

        this.forceUpdate();
    }

  renderHospitalTable(hospitals) {
    return [
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Name</th>
            <th>Description</th>
            <th>Address</th>
            <th>Phone Number</th>
                    <th><button onClick={() => this.setState({ showEdit: true })}>Add</button></th>
          </tr>
        </thead>
        <tbody>
          {hospitals.map(hospital =>
            <tr key={hospital.hospitalId}>
              <td>{hospital.name}</td>
              <td>{hospital.description}</td>
              <td>{parse(hospital.addressDisplayHtml)}</td>
              <td>{hospital.phoneNumber}</td>
                  <td><button>Contacts</button>
                      <br /><button onClick={async () => { this.setState({ showEdit: true }); this.hospitalId.current = hospital.hospitalId; await this.populateFormData(); }}>Edit</button>
                      <br /><button onClick={async () => { this.hospitalId.current = hospital.hospitalId; await this.deleteHospital(); } }>Delete</button>
                  </td>
            </tr>
          )}
        </tbody>
        </table>

        , this.state.showEdit
        ? < div className = "container" >
            <div className="row"><div className="col-12"><button onClick={() => this.setState({ showEdit: false })}>Close Dialog</button></div></div>
            <div className="row"><div className="col-12"><h3>{this.hospitalId.current && this.hospitalId.current !== 0 ? "Edit" : "Add"} Hospital</h3></div></div>
            
            <form onSubmit={this.addEditHospital}>
                <div className="row">
                    <div className="col-3">
                        <input ref={this.name} type="text" required="required" name="name" placeholder="Hospital name"/>
                    </div>
                    <div className="col-3">
                        <input ref={this.description} type="text" name="description" placeholder="Hospital description" />
                    </div>
                    <div className="col-3">
                        <input ref={this.addressLine1} type="text" required="required" name="addressLine1" placeholder="Address line 1" />
                        <input ref={this.addressLine2} type="text" name="addressLine2" placeholder="Address line 2" />
                        <input ref={this.addressLine3} type="text" name="addressLine3" placeholder="Address line 3" />
                        <input ref={this.addressCity} type="text" required="required" name="addressCity" placeholder="City" />
                        <input ref={this.addressState} type="text" required="required" name="addressState" placeholder="State" />
                        <input ref={this.addressZip} type="text" required="required" name="addressZip" placeholder="ZIP code" />
                    </div>
                    <div className="col-3">
                        <input ref={this.phoneNumber} type="tel" name="phoneNumber" placeholder="Phone number" />
                    </div>
                </div>
                <div className="row"><div className="col-12"><button type="submit">Save</button></div></div>
                <div className="row"><div className="col-12"><button onClick={this.resetForm}>Clear</button></div></div>
            </form>
            </div> 
            : null
        
    ];
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : this.renderHospitalTable(this.state.hospitals);

    return (
      <div>
        <h1 id="tabelLabel">Hospitals</h1>
            {contents}
      </div>
    );
  }

  async populateHospitalData() {
    //const token = await authService.getAccessToken();
    const response = await fetch('api/hospitals', {
      //headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });
    const data = await response.json();
    this.setState({ hospitals: data, loading: false });
  }

    async populateFormData() {
        //const token = await authService.getAccessToken();
        const response = await fetch('api/hospitals', {
            //headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        });
        const data = await response.json();
        let hospital = data.filter(h => h.hospitalId === this.hospitalId.current)[0];

        this.hospitalId.current = hospital.hospitalId;
        this.name.current.value = hospital.name;
        this.description.current.value = hospital.description;
        this.addressLine1.current.value = hospital.addressLine1;
        this.addressLine2.current.value = hospital.addressLine2;
        this.addressLine3.current.value = hospital.addressLine3;
        this.addressCity.current.value = hospital.addressCity;
        this.addressState.current.value = hospital.addressState;
        this.addressZip.current.value = hospital.addressZip;
        this.phoneNumber.current.value = hospital.phoneNumber;

        this.forceUpdate();
    }

    addEditHospital = async (e) => {
        e.preventDefault();
        const data = {
            hospitalId: this.hospitalId.current,
            name: this.name.current.value,
            description: this.description.current.value,
            addressLine1: this.addressLine1.current.value,
            addressLine2: this.addressLine2.current.value,
            addressLine3: this.addressLine3.current.value,
            addressCity: this.addressCity.current.value,
            addressState: this.addressState.current.value,
            addressZip: this.addressZip.current.value,
            phoneNumber: this.phoneNumber.current.value
        };

        const options = {
            method: "POST",
            contentType: "application/json",
            body: JSON.stringify(data)
        };
        await fetch("api/hospitals/edit", options)

        this.populateHospitalData();

        this.resetForm();
    }

    async deleteHospital() {
        await fetch("api/hospitals/delete/" + this.hospitalId.current, {
            method: "DELETE"
        });

        this.populateHospitalData();
        this.resetForm();
    }
}
